using System.Diagnostics;
using ASP_P42.Models;
using ASP_P42.Services.Hash;
using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers
{
    // primary constructor - прямо при оголошенні класу
    public class HomeController(IHashService hashService) : Controller
    {
        // Інжекція через конструктор у формі Primary
        private readonly IHashService _hashService = hashService;

        public IActionResult IoC()
        {
            String digest = _hashService.Digest("123");
            // передача даних до представлення
            // варіанти спільних ресурсів
            ViewBag.Hash = _hashService.GetHashCode();
            ViewData["digest"] = digest;
            return View();
        }

        public IActionResult Razor()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Intro()
        {
            return View();
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
