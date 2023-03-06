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
            return View(new BrickCalculationInput
            {
                Apertures = new[] { new ApertureInput()},
                Workers = new []{new WorkerInput()}
            });
        }
        
        [HttpGet]
        public IActionResult BalkCalculatorIndex()
        {
            return View(new BalkCalculationInput
            {
                Apertures = new[] { new ApertureInput()},
                Workers = new []{new WorkerInput()} 
            });
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
        public IActionResult GetDocument([FromForm] BrickCalculationInput input)
        {
            var result = _wordGenerator.Generate(input);

            return File(result.Content, result.ContentType, result.FileName);
        }
    }
}
