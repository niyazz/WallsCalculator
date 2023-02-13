using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WallsCalculator.Models;

namespace WallsCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult BrickCalculator()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Calculate([FromForm] BrickCalculationInput input)
        {
            return View("Views/Home/BrickCalculator.cshtml");
        }

        public IActionResult HideCalculator()
        {
            if (TempData["Hidden"] == null)
            {
                TempData["Hidden"] = true;
            }

            else
            {
                TempData["Hidden"] = !(bool)TempData["Hidden"];
            }

            return View("Views/Home/BrickCalculator.cshtml");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}