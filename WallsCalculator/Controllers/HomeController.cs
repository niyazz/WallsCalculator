using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WallsCalculator.Models;
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