using System;
using System.IO;
using System.Linq;
using Spire.Doc;
using WallsCalculator.Models;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Services.Abstractions;
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

        public HttpFileContent Generate(BrickCalculationInput calculatorInput, string fileName)
        {
            var calculated = _calculator.Calculate(calculatorInput)!;
            var builder = new DocumentFormatBuilder();
            var tableIndex = 1;

            var firstPage = AddPage(builder)
                .AddNiceText($"Расчет кирпичной стены от {DateTime.Now:dd/MM/yyyy}", BigHeading, MidLineSpacing)
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
                .AddNiceText("Площадь, которой не нужна кладка.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Общая площадь всех проёмов (окон и дверей), где не нужна кладка.", JustifyText, MidLineSpacing)
                .AddNiceText("Вид кирпича.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Вид кирпича выбранного для расчета.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество выбранного типа кирпича и толщины в 1 м².", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Сколько кирпича в 1 м² кладки – в зависимости от видов материала и толщины шва.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество кирпича необходимого для кладки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Общее количество кирпичей необходимых для постройки стен по заданным параметрам.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество кирпичей в колонне.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Расчетное количество кирпичей в колонне в зависимости от высоты стены.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость одного кирпича.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Стоимость единицы материала выбранного вида кирпича.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость кирпичей.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Стоимость материалов для возведения кирпичной стены.", JustifyText, MidLineSpacing)
                .AddNiceText("Толщина раствора.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Высота шва между кирпичами.", JustifyText, MidLineSpacing)
                .AddNiceText("Тип кладки кирпича.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Способ кладки изделия для возведения стены.", JustifyText, MidLineSpacing)
                .AddNiceText("Тип кладки сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Способ покрытия кладочной сетки для возведения стены.", JustifyText, MidLineSpacing)
                .AddNiceText("Число рядов кладочной сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Количество рядов кладочной сетки, которое будет необходимо.", JustifyText, MidLineSpacing)
                .AddNiceText("Площадь необходимой кладочной сетки.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Площадь кладочной сетки, которую нужна для кладки.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость найма рабочих.", JustifyTextBold, MidLineSpacing)
                    .AddNiceText("- Стоимость найма всех работников, которые будут наняты для работы.", JustifyText, MidLineSpacing);

            var lastPage = AddPage(builder, addStyle:false);
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
                .FillRowWith("Вид крипича", input.BrickType.GetMaterialDescription(input.BrickType.GetEnumDisplayName())!)
                .FillRowWith("Тип кладки кирпича", input.DepthType.GetEnumDisplayName())
                .FillRowWith("Тип кладки сетки", input.MasonryType.GetEnumDisplayName())
                .FillRowWith("Толщина раствора", $"{input.MortarValue} мм.")
                .FillRowWith("Стоимость одного кирпича", $"{input.Price} руб.")
                .EndNiceTable();
            return tableIndex;
        }

        private int AddResultsTable(BrickCalculationOutput output, Section lastPage, int tableIndex)
        {
            AddBaseResultsTable(output, lastPage, ref tableIndex, 11, 2)
                .FillRowWith("Количество выбранного типа кирпича и толщины в 1 м²", $"{output.OneSquareBricksAmount} шт.")
                .FillRowWith("Площадь необходимой кладочной сетки", $"{output.AreaForMasonryGrid} м².")
                .FillRowWith("Число рядов кладочной сетки", $"{output.MasonryGridRowsAmount} шт.")
                .FillRowWith("Количество кирпичей в колонне", $"{output.ColumnBricksAmount} шт.")
                .EndNiceTable();
            return tableIndex;
        }
    }
}