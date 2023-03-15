using System;
using System.IO;
using System.Linq;
using Spire.Doc;
using WallsCalculator.Models;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Utils;
using static WallsCalculator.Utils.NiceStyles;
// ReSharper disable HeapView.BoxingAllocation

namespace WallsCalculator.Services.WordGenerators
{
    public class BrickWordGeneratorService 
        : BaseWordGeneratorDocumentService, IWordGeneratorDocumentService<BrickCalculationInput>
    {
        private readonly ICalculator<BrickCalculationInput, BrickCalculationOutput> _calculator;

        public BrickWordGeneratorService(
            ICalculator<BrickCalculationInput, BrickCalculationOutput> calculator)
        {
            _calculator = calculator;
        }

        public HttpFileContent Generate(BrickCalculationInput calculatorInput,
            string fileName = "Результат расчёта новые")
        {
            var calculated = _calculator.Calculate(calculatorInput)!;
            var builder = new DocumentFormatBuilder();
            var tableIndex = 1;
            
            var lastPage = AddPage(builder)
                .AddNiceText($"Расчет кирпичной стены от {DateTime.Now:dd/MM/yyyy}\n", BigHeading, MidLineSpacing);
            tableIndex = AddInputsTable(calculated, lastPage, tableIndex);
            tableIndex = AddAperturesTable(calculated.Input.Apertures.ToArray(), lastPage, ref tableIndex);
            tableIndex = AddWorkersTable(calculated.Input.Workers.ToArray(), lastPage, ref tableIndex);
            _ = AddResultsTable(calculated, lastPage, tableIndex);

            var document = builder.Build();
            var stream = new MemoryStream();
            document.SaveToStream(stream, FileFormat.Doc);

            return new()
            {
                FileName = fileName.LastIndexOf(".") == -1
                    ? $"{fileName}.doc"
                    : $"{fileName[..fileName.LastIndexOf('.')]}.doc",
                Content = stream.ToArray(),
                ContentType = MsWord
            };
        }

        private int AddInputsTable(BrickCalculationOutput output, Section lastPage, int tableIndex)
        {
            var input = output.Input;
            AddBaseInputsTable(output.Input, output, lastPage, ref tableIndex, 8, 2)
                .FillRowWith("Вид крипича", input.BrickType.GetBrickDescription())
                .FillRowWith("Тип кладки кирпича", input.DepthType.GetEnumDisplayName())
                .FillRowWith("Тип кладки сетки", input.MasonryType.GetEnumDisplayName())
                .FillRowWith("Толщина раствора", $"{input.MortarValue} мм.")
                .FillRowWith("Цена кирпича", $"{input.Price} руб.")
                .EndNiceTable();
            return tableIndex;
        }

        private int AddResultsTable(BrickCalculationOutput output, Section lastPage, int tableIndex)
        {
            AddBaseResultsTable(output, lastPage, ref tableIndex, 11, 2)
                .FillRowWith("Количество кирпича для кладки одного квадратного метра", $"{output.OneSquareBricksAmount} шт.")
                .FillRowWith("Площадь кладочной сетки", $"{output.AreaForMasonryGrid} кв.м.")
                .FillRowWith("Число рядов кладочной сетки", $"{output.MasonryGridRowsAmount} шт.")
                .FillRowWith("Число кирпичей в колонне", $"{output.ColumnBricksAmount} шт.")
                .EndNiceTable();
            return tableIndex;
        }
    }
}