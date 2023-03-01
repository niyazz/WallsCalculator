using Microsoft.AspNetCore.Mvc;
using WallsCalculator.Models;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Services;

namespace WallsCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly IBrickCalculator _calculator;
        private readonly IWordGeneratorDocumentService _wordGenerator;
        public CalculatorController(IBrickCalculator calculator, IWordGeneratorDocumentService wordGenerator)
        {
            _calculator = calculator;
            _wordGenerator = wordGenerator;
        }
        
        [HttpGet]
        public IActionResult BrickCalculatorIndex()
        {
            return View(new BrickCalculationInput { Apertures = new[] { new ApertureInput()} });
        }
        
        [HttpGet]
        public IActionResult BalkCalculatorIndex()
        {
            return View(new BalkCalculationInput { Apertures = new[] { new ApertureInput()} });
        }
        
        [HttpPost]
        public IActionResult BrickCalculatorIndex([FromForm] BrickCalculationInput input)
        {
            if (ModelState.IsValid)
            {
                var resultModel = _calculator.Calculate(input);
                if (resultModel != null)
                {
                    resultModel.Input = input;
                    ViewData["Result"] = resultModel;
                }
                ViewBag.IsCalculated = resultModel != null;
            }

            return View(input);
        }

        [HttpPost]
        public IActionResult GetDocument([FromForm] BrickCalculationOutput output)
        {
            var result = _wordGenerator.Generate(output);

            return File(result.Item2, result.Item3, result.Item1);
        }
    }
}
