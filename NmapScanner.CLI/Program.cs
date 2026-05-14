using System;
using NmapScanner.Core;

namespace NmapScanner.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Nmap Security Scanner ===");
            Console.Write("Введіть IP-адресу для сканування (наприклад, 192.168.1.1): ");
            string targetIp = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(targetIp))
            {
                Console.WriteLine("Помилка: IP-адреса не може бути порожньою.");
                return;
            }

            try
            {
                var scanner = new Scanner();
                ScanResult result = scanner.RunScan(targetIp);

                Console.WriteLine("\n=== Результати сканування ===");
                Console.WriteLine($"Ціль: {result.IpAddress}");
                Console.WriteLine($"Знайдено відкритих/відфільтрованих портів: {result.Ports.Count}");
                Console.WriteLine(new string('-', 75));
                Console.WriteLine($"{"Порт",-8} | {"Стан",-10} | {"Сервіс",-15} | {"Версія",-20}");
                Console.WriteLine(new string('-', 75));

                foreach (var port in result.Ports)
                {
                    Console.WriteLine($"{port.PortId,-8} | {port.State,-10} | {port.ServiceName,-15} | {port.ServiceVersion}");
                }
                Console.WriteLine(new string('-', 75));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ПОМИЛКА] {ex.Message}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}