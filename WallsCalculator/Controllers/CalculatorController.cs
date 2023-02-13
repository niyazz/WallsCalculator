using Microsoft.AspNetCore.Mvc;
using WallsCalculator.Models;

namespace WallsCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        private const string BrickCalculatorPage = "Views/Home/BrickCalculator.cshtml";

        public IActionResult GetBricksResult(
            [FromForm]BrickCalculationInput input)
        {
            return View(BrickCalculatorPage);
        }
    }
}
