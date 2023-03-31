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
                .AddNiceText($"Расчет стены из блока {DateTime.Now:dd/MM/yyyy}\n", BigHeading, MidLineSpacing)
                .AddNiceText("\nОбщая площадь", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Сумма площади внезависимости от необходимости кладки.", JustifyText, MidLineSpacing)
                .AddNiceText("\nПлощадь кладки", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Площадь внешней стороны стен и соответствует площади необходимого утеплителя.", JustifyText, MidLineSpacing)
                .AddNiceText("Общая длина всех стен.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Периметр всех стен.", JustifyText, MidLineSpacing)
                .AddNiceText("Высота стен по углам.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Высота потолков в углах стен. Используется среднее значение.", JustifyText, MidLineSpacing)
                .AddNiceText("Толщина стены.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Толщина готовой стены с учетом толщины растворного шва. Может незначительно отличаться от конечного результата в зависимости от вида кладки.", JustifyText, MidLineSpacing)
                .AddNiceText("Общий вес конструкции.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Вес всех стен в совокупности в зависимости от типа выбранного блока.", JustifyText, MidLineSpacing)
                .AddNiceText("Площадь, которой не нужна кладка.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Общая площадь всех проёмов (окон и дверей), где не нужна кладка.", JustifyText, MidLineSpacing)
                .AddNiceText("Вид блока.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Вид блока выбранного для расчета.", JustifyText, MidLineSpacing)
                .AddNiceText("Вес блока.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Вес блока в килограммах.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество выбранного типа блока и толщины в 1 м².", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Сколько блока в 1 м² кладки – в зависимости от видов материала и толщины шва.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество блоков необходимого для кладки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Общее количество блоков необходимых для постройки стен по заданным параметрам.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество блоков в колонне.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Расчетное количество блоков в колонне в зависимости от высоты стены.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость одного блока.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Стоимость единицы материала выбранного вида блока.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость блоков.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Стоимость материалов для возведения стены из блока.", JustifyText, MidLineSpacing)
                .AddNiceText("Толщина раствора.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Высота шва между блоками.", JustifyText, MidLineSpacing)
                .AddNiceText("Тип кладки блоков.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Способ кладки изделия для возведения стены.", JustifyText, MidLineSpacing)
                .AddNiceText("Тип кладки сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Способ покрытия кладочной сетки для возведения стены.", JustifyText, MidLineSpacing)
                .AddNiceText("Число рядов кладочной сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Количество рядов кладочной сетки, которое будет необходимо.", JustifyText, MidLineSpacing)
                .AddNiceText("Площадь необходимой кладочной сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Площадь кладочной сетки, которую нужна для кладки.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость найма рабочих.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Стоимость найма всех работников, которые будут наняты для работы.", JustifyText, MidLineSpacing);
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
                .FillRowWith("Стоимость одного блока", $"{input.Price} руб.")
                .EndNiceTable();
            return tableIndex;
        }

        private int AddResultsTable(BlockCalculationOutput output, Section lastPage, int tableIndex)
        {
            AddBaseResultsTable(output, lastPage, ref tableIndex, 12, 2)
                .FillRowWith("Количество выбранного типа блока и толщины в 1 м²", $"{output.OneSquareBlocksAmount} шт.")
                .FillRowWith("Площадь необходимой кладочной сетки", $"{output.AreaForMasonryGrid} м².")
                .FillRowWith("Число рядов кладочной сетки", $"{output.MasonryGridRowsAmount} шт.")
                .FillRowWith("Количество блоков в колонне", $"{output.ColumnBlocksAmount} шт.")
                .FillRowWith("Общий вес конструкции", $"{output.ConstructionWeight} шт.")
                .EndNiceTable();
            return tableIndex;
        }
    }
}