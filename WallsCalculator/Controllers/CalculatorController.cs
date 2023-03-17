using Microsoft.AspNetCore.Mvc;
using WallsCalculator.Models;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Services;
using WallsCalculator.Services.Abstractions;
using WallsCalculator.Utils;

namespace WallsCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly EditableSettings _settings;
        public CalculatorController(EditableSettings settings)
        {
            _settings = settings;
        }
        
        #region Ввод данных для расчёта

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
        
        [HttpGet]
        public IActionResult BlockCalculatorIndex()
        {
            return View(new BlockCalculationInput
            {
                Apertures = new[] { new ApertureInput()},
                Workers = new []{new WorkerInput()} 
            });
        }

        #endregion

        #region Получение результатов расчетов

        [HttpPost]
        public IActionResult BrickCalculatorIndex(
            [FromServices] ICalculator<BrickCalculationInput, BrickCalculationOutput> brickCalculator,
            [FromForm] BrickCalculationInput input)
        {
            if (ModelState.IsValid)
            {
                var resultModel = brickCalculator.Calculate(input);
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
        public IActionResult BalkCalculatorIndex(
            [FromServices] ICalculator<BalkCalculationInput, BalkCalculationOutput> balkCalculator,
            [FromForm] BalkCalculationInput input)
        {
            if (ModelState.IsValid)
            {
                var resultModel = balkCalculator.Calculate(input);
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
        public IActionResult BlockCalculatorIndex(
            [FromServices] ICalculator<BlockCalculationInput, BlockCalculationOutput> balkCalculator,
            [FromForm] BlockCalculationInput input)
        {
            if (ModelState.IsValid)
            {
                var resultModel = balkCalculator.Calculate(input);
                if (resultModel != null)
                {
                    resultModel.Input = input;
                    ViewData["Result"] = resultModel;
                }
                ViewBag.IsCalculated = resultModel != null;
            }

            return View(input);
        }

        #endregion

        #region Формирование документов Word
        
        [ActionName("BrickGetDocument")]
        [HttpPost]
        public IActionResult GetDocument(
            [FromServices] IWordGeneratorDocumentService<BrickCalculationInput> brickWordGenerator,
            [FromForm] BrickCalculationInput input)
        {
            var result = brickWordGenerator.Generate(input, _settings.BrickDocumentName);

            return File(result.Content, result.ContentType, result.FileName);
        }
        
        [ActionName("BalkGetDocument")]
        [HttpPost]
        public IActionResult GetDocument(
            [FromServices] IWordGeneratorDocumentService<BalkCalculationInput> balkWordGenerator,
            [FromForm] BalkCalculationInput input)
        {
            var result = balkWordGenerator.Generate(input, _settings.BalkDocumentName);

            return File(result.Content, result.ContentType, result.FileName);
        }
        
        [ActionName("BlockGetDocument")]
        [HttpPost]
        public IActionResult GetDocument(
            [FromServices] IWordGeneratorDocumentService<BlockCalculationInput> balkWordGenerator,
            [FromForm] BlockCalculationInput input)
        {
            var result = balkWordGenerator.Generate(input, _settings.BlockDocumentName);

            return File(result.Content, result.ContentType, result.FileName);
        }

        #endregion
    }
}
