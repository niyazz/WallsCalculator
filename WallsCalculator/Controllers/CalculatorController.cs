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
            return View();
        }


        [HttpPost]
        public IActionResult BrickCalculatorIndex([FromForm] BrickCalculationInput input, string TextBox1)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("IsValid");
            }

            return View(input);
        }

        [HttpPost]
        public IActionResult Add([FromForm] BrickCalculationInput input, string TextBox1)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("IsValid");
            }

            return View("Views/Calculator/BrickCalculatorIndex.cshtml", input);
        }
    }
}
