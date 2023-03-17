using System;
using System.IO;
using System.Linq;
using Spire.Doc;
using WallsCalculator.Models;
using WallsCalculator.Services.Abstractions;
using WallsCalculator.Utils;
using static WallsCalculator.Utils.NiceStyles;
// ReSharper disable HeapView.BoxingAllocation

namespace WallsCalculator.Services.WordGenerators
{
    public class BalkWordGeneratorService 
        : BaseWordGeneratorDocumentService, IWordGeneratorDocumentService<BalkCalculationInput>
    {
        private readonly ICalculator<BalkCalculationInput, BalkCalculationOutput> _calculator;

        public BalkWordGeneratorService(
            ICalculator<BalkCalculationInput, BalkCalculationOutput> calculator)
        {
            _calculator = calculator;
        }

        public HttpFileContent Generate(BalkCalculationInput calculatorInput, string fileName)
        {
            var calculated = _calculator.Calculate(calculatorInput)!;
            var builder = new DocumentFormatBuilder();
            var tableIndex = 1;
            
            var lastPage = AddPage(builder)
                .AddNiceText($"Расчет стены из бруса от {DateTime.Now:dd/MM/yyyy}\n", BigHeading, MidLineSpacing);
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

        private int AddInputsTable(BalkCalculationOutput output, Section lastPage, int tableIndex)
        {
            var input = output.Input;
            AddBaseInputsTable(output.Input, output, lastPage, ref tableIndex, 8, 2)
                .FillRowWith("Тип бруса", input.BalkType.GetMaterialDescription(input.BalkType.GetEnumDisplayName())!)
                .FillRowWith("Ширина бруса", $"{input.BalkWidth} мм")
                .FillRowWith("Высота бруса", $"{input.BalkHeight} мм")
                .FillRowWith("Длина бруса", $"{input.BalkLength} м")
                .FillRowWith("Цена за куб", $"{input.Price} руб.")
                .EndNiceTable();
            return tableIndex;
        }

        private int AddResultsTable(BalkCalculationOutput output, Section lastPage, int tableIndex)
        {
            AddBaseResultsTable(output, lastPage, ref tableIndex, 12, 2)
                .FillRowWith("Объем бруса на дом в куб.метре", $"{output.AreaToCoverCubeM} куб.м.")
                .FillRowWith("Количество бруса в одном куб.метре.", $"{output.OneCubeBalkAmount} шт.")
                .FillRowWith("Объем бруса в куб.метрах", $"{output.BalkVolumeCubeM} куб.м.")
                .FillRowWith("Число рядов бруса", $"{output.BalkRowsAmount} шт.")
                .FillRowWith("Вес конструкции из бруса", $"{output.ConstructionWeight} кг.")
                .EndNiceTable();
            return tableIndex;
        }
    }
}