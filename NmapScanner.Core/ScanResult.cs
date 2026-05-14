using System.Collections.Generic;

namespace NmapScanner.Core
{
    public class ScanResult
    {
        public string IpAddress { get; set; }
        // Список всіх знайдених портів
        public List<PortInfo> Ports { get; set; } = new List<PortInfo>();
    }
}