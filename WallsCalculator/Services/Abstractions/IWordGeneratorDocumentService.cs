using WallsCalculator.Models;

namespace WallsCalculator.Services.Abstractions
{
    public interface IWordGeneratorDocumentService<in TInput> 
        where TInput : CalculationInput
    {
        /// <summary>
        /// Сгенерировать Word документ. 
        /// </summary>
        /// <param name="calculatorInput">Входные данные калькулятора.</param>
        /// <param name="fileName">Название выходного файла без расширения.</param>
        HttpFileContent Generate(TInput calculatorInput, string fileName);
    }
}