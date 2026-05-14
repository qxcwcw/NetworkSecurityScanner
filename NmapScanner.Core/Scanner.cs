using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace NmapScanner.Core
{
    public class Scanner
    {
        public ScanResult RunScan(string ipAddress)
        {
            string outputFileName = "scan_result.xml";

            // 1. Формуємо команду для запуску реального nmap
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = "nmap",
                Arguments = $"-sV -oX {outputFileName} {ipAddress}",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Console.WriteLine($"[ЯДРО] Запуск Nmap для {ipAddress}... Це може зайняти хвилину.");

            // 2. Запускаємо процес
            using (Process process = Process.Start(processInfo))
            {
                process.WaitForExit(); // Чекаємо, поки nmap повністю завершить роботу
            }

            // 3. Перевіряємо, чи створився файл
            if (!File.Exists(outputFileName))
            {
                throw new Exception("Файл з результатами не був створений. Можливо, Nmap не встановлений або виникла помилка.");
            }

            // 4. Парсимо реальний XML
            return ParseXmlResults(outputFileName, ipAddress);
        }

        private ScanResult ParseXmlResults(string filePath, string ipAddress)
        {
            var result = new ScanResult { IpAddress = ipAddress };

            // Завантажуємо згенерований файл
            XDocument xmlDoc = XDocument.Load(filePath);

            // Шукаємо всі теги <port>
            var ports = xmlDoc.Descendants("port");

            foreach (var port in ports)
            {
                var portInfo = new PortInfo
                {
                    PortId = port.Attribute("portid")?.Value,
                    State = port.Element("state")?.Attribute("state")?.Value
                };

                // Витягуємо інформацію про сервіс та версію (прапорець -sV)
                var service = port.Element("service");
                if (service != null)
                {
                    portInfo.ServiceName = service.Attribute("name")?.Value ?? "unknown";

                    // Версія може складатися з продукту і самої версії
                    string product = service.Attribute("product")?.Value ?? "";
                    string version = service.Attribute("version")?.Value ?? "";

                    portInfo.ServiceVersion = string.IsNullOrWhiteSpace(product) ? "Не визначено" : $"{product} {version}".Trim();
                }

                result.Ports.Add(portInfo);
            }

            return result;
        }
    }
}