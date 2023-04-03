using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WallsCalculator.Models.Shared;
using WallsCalculator.Utils;

namespace WallsCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly EditableSettings _settings;
        public HomeController(EditableSettings settings)
        {
            _settings = settings;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.Author = _settings.Author;
            return View();
        }

        public IActionResult Calculator()
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