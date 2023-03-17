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
    public class BlockWordGeneratorService 
        : BaseWordGeneratorDocumentService, IWordGeneratorDocumentService<BlockCalculationInput>
    {
        private readonly ICalculator<BlockCalculationInput, BlockCalculationOutput> _calculator;

        public BlockWordGeneratorService(
            ICalculator<BlockCalculationInput, BlockCalculationOutput> calculator)
        {
            _calculator = calculator;
        }

        public HttpFileContent Generate(BlockCalculationInput calculatorInput, string fileName)
        {
            var calculated = _calculator.Calculate(calculatorInput)!;
            var builder = new DocumentFormatBuilder();
            var tableIndex = 1;
            
            var lastPage = AddPage(builder)
                .AddNiceText($"Расчет стены из блока {DateTime.Now:dd/MM/yyyy}\n", BigHeading, MidLineSpacing);
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

        private int AddInputsTable(BlockCalculationOutput output, Section lastPage, int tableIndex)
        {
            var input = output.Input;
            AddBaseInputsTable(output.Input, output, lastPage, ref tableIndex, 9, 2)
                .FillRowWith("Вид блока", input.BlockType.GetMaterialDescription(input.BlockType.GetEnumDisplayName())!)
                .FillRowWith("Тип кладки блока", input.DepthType.GetEnumDisplayName())
                .FillRowWith("Тип кладки сетки", input.MasonryType.GetEnumDisplayName())
                .FillRowWith("Толщина раствора", $"{input.MortarValue} мм.")
                .FillRowWith("Вес блока", $"{input.BlockWeight} мм.")
                .FillRowWith("Цена блока", $"{input.Price} руб.")
                .EndNiceTable();
            return tableIndex;
        }

        private int AddResultsTable(BlockCalculationOutput output, Section lastPage, int tableIndex)
        {
            AddBaseResultsTable(output, lastPage, ref tableIndex, 12, 2)
                .FillRowWith("Количество блока для кладки одного квадратного метра", $"{output.OneSquareBlocksAmount} шт.")
                .FillRowWith("Площадь кладочной сетки", $"{output.AreaForMasonryGrid} кв.м.")
                .FillRowWith("Число рядов кладочной сетки", $"{output.MasonryGridRowsAmount} шт.")
                .FillRowWith("Число блока в колонне", $"{output.ColumnBlocksAmount} шт.")
                .FillRowWith("Общий вес конструкции", $"{output.ConstructionWeight} шт.")
                .EndNiceTable();
            return tableIndex;
        }
    }
}