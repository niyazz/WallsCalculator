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

            var firstPage = AddPage(builder)
                .AddNiceText($"Расчет стены из блока {DateTime.Now:dd/MM/yyyy}\n", BigHeading, MidLineSpacing);
            tableIndex = AddInputsTable(calculated, firstPage, tableIndex);
            tableIndex = AddAperturesTable(calculated.Input.Apertures.ToArray(), firstPage, ref tableIndex);
            tableIndex = AddWorkersTable(calculated.Input.Workers.ToArray(), firstPage, ref tableIndex);
            _ = AddResultsTable(calculated, firstPage, tableIndex);

            #region Общие сведения
            
            //var lastPage = AddPage(builder, addStyle:false);
            firstPage
                .AddNiceText("\nОбщие сведения по результатам расчетов", BigHeading, MidLineSpacing)
                .AddNiceText("\nОбщая площадь", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Сумма всех площадей помещений дома.", JustifyText, MidLineSpacing)
                .AddNiceText("Площадь кладки", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Площадь внешней стороны стен и соответствует площади необходимого утеплителя.", JustifyText, MidLineSpacing)
                .AddNiceText("Общая длина всех стен.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Сумма длин стен всех помещений дома.", JustifyText, MidLineSpacing)
                .AddNiceText("Высота стен по углам.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Высота стен дома.", JustifyText, MidLineSpacing)
                .AddNiceText("Толщина стены.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Толщина готовой стены с учетом толщины растворного шва. Может незначительно отличаться от конечного результата в зависимости от вида кладки.", JustifyText, MidLineSpacing)
                .AddNiceText("Общий вес конструкции.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Вес нагрузки конструкции.", JustifyText, MidLineSpacing)
                .AddNiceText("Площадь, которой не нужна кладка.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Общая площадь всех проёмов (окон и дверей), где не нужна кладка.", JustifyText, MidLineSpacing)
                .AddNiceText("Вид блока.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Разновидности блоков: Керамзитобетонные, Газобетонные, Пенобетонные.", JustifyText, MidLineSpacing)
                .AddNiceText("Вес блока.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Вес одного блока.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество выбранного типа блока и толщины в 1 м².", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Сколько блоков в 1 м² кладки – в зависимости от видов материала и толщины шва.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество блока необходимого для кладки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Общее количество блоков необходимых для постройки стен по заданным параметрам.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество блоков в колонне.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Необходимое количество блоков в колонне.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость одного блока.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Цена за один блок.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость блоков.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Стоимость блоков для возведения блочной стены.", JustifyText, MidLineSpacing)
                .AddNiceText("Толщина раствора.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Толщина раствора между блоками.", JustifyText, MidLineSpacing)
                .AddNiceText("Тип кладки блока.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Типы кладки блока могут быть в пол изделия, в 1 изделие, в 1,5 изделия, в 2 изделия.", JustifyText, MidLineSpacing)
                .AddNiceText("Тип кладки сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Типы кладки сетки могут быть: каждый ряд, через ряд, через 2 ряда, через 3 ряда, через 4 ряда.", JustifyText, MidLineSpacing)
                .AddNiceText("Число рядов кладочной сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Количество рядов кладочной сетки, которое будет необходимо.", JustifyText, MidLineSpacing)
                .AddNiceText("Площадь необходимой кладочной сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Площадь кладочной сетки, которую нужна для кладки.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость найма рабочих.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Стоимость найма всех работников, которые будут наняты для работы.", JustifyText, MidLineSpacing);

            #endregion

            #region Создание документа

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

            #endregion
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
                .FillRowWith("Площадь кладки", $"{output.AreaToCoverSquareM} м²")
                .FillRowWith("Площадь, которой не нужна кладка", output.AreaToNotCoverSquareM > 0 ? $"{output.AreaToNotCoverSquareM} м²" : "Без проемов")
                .FillRowWith("Количество выбранного типа блока и толщины в 1 м²", $"{output.OneSquareBlocksAmount} шт.")
                .FillRowWith("Количество блока необходимого для кладки", $"{output.TotalMaterialAmount} шт.")
                .FillRowWith("Количество блоков в колонне", $"{output.ColumnBlocksAmount} шт.")
                .FillRowWith("Стоимость блоков", $"{output.TotalMaterialPrice} руб.")
                .FillRowWith("Число рядов кладочной сетки", $"{output.MasonryGridRowsAmount} шт.")
                .FillRowWith("Площадь необходимой кладочной сетки", $"{output.AreaForMasonryGrid} м²")
                .FillRowWith("Общий вес конструкции", $"{output.ConstructionWeight} кг")
                .FillRowWith("Стоимость найма рабочих", output.AllWorkersPrice > 0 ? $"{output.AllWorkersPrice} руб." : "Без найма")
                .FillRowWith("Итоговая стоимость работ с учетом найма работников и закупки материалов", $"{output.TotalMaterialAndWorkersPrice} руб.")
                .EndNiceTable();
            return tableIndex;
        }
    }
}