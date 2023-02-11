using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public IActionResult Calculator()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Calculate([FromForm] CalculationBaseInput input)
        {
            ViewBag.Result = input.X + input.Y;
            return View("Views/Home/Calculator.cshtml");
        }

        [HttpGet]
        public IActionResult HideCalculator()
        {
            if(ViewBag.Hidden == null)
            {
                ViewBag.Hidden = true;
            }

            else
            {
                ViewBag.Hidden = !ViewBag.Hidden;
            }

            return View("Views/Home/Calculator.cshtml");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}