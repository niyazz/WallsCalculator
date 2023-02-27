using Microsoft.AspNetCore.Mvc;
using System;
using WallsCalculator.Models;

namespace WallsCalculator.Controllers
{
    public class CalculatorController : Controller
    {

        [HttpGet]
        public IActionResult BrickCalculatorIndex()
        {
            return View(new BrickCalculationInput { Apertures = new[] { new Aperture()} });
        }


        [HttpPost]
        public IActionResult BrickCalculatorIndex([FromForm] BrickCalculationInput input)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("IsValid");
            }

            return View(input);
        }
    }
}
