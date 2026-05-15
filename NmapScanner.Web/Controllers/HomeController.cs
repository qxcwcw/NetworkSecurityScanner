using Microsoft.AspNetCore.Mvc;
using NmapScanner.Web.Models;
using System.Diagnostics;
using NmapScanner.Core; // Підключаємо наше Ядро!

namespace NmapScanner.Web.Controllers
{
    public class HomeController : Controller
    {
        // Відкриває головну сторінку
        public IActionResult Index()
        {
            return View();
        }

        // Спрацьовує, коли ми натискаємо кнопку "Сканувати"
        [HttpPost]
        public IActionResult Index(string targetIp)
        {
            if (string.IsNullOrWhiteSpace(targetIp))
            {
                ViewBag.Error = "IP-адреса не може бути порожньою.";
                return View();
            }

            try
            {
                var scanner = new Scanner();
                var result = scanner.RunScan(targetIp);
                return View(result); // Передаємо результати на сторінку
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Помилка: {ex.Message}";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}