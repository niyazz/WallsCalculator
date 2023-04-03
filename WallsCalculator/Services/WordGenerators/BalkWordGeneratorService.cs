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

            var firstPage = AddPage(builder).AddNiceText($"Расчет стены из бруса от {DateTime.Now:dd/MM/yyyy}", BigHeading, MidLineSpacing);
            tableIndex = AddInputsTable(calculated, firstPage, tableIndex);
            tableIndex = AddAperturesTable(calculated.Input.Apertures.ToArray(), firstPage, ref tableIndex);
            tableIndex = AddWorkersTable(calculated.Input.Workers.ToArray(), firstPage, ref tableIndex);
            _ = AddResultsTable(calculated, firstPage, tableIndex);

            #region Общие сведения

            //var lastPage = AddPage(builder, addStyle:false);
            firstPage
                .AddNiceText("\nОбщие сведения по результатам расчетов", BigHeading, MidLineSpacing)
                .AddNiceText("Площадь кладки", JustifyTextBold, MidLineSpacing)
                .AddNiceText("- Площадь внешней стороны стен и соответствует площади необходимого утеплителя.",
                    JustifyText, MidLineSpacing)
                .AddNiceText("Площадь, которой не нужна кладка.", JustifyTextBold, MidLineSpacing)
                .AddNiceText("- Общая площадь всех проёмов (окон и дверей), где не нужна кладка.", JustifyText,
                    MidLineSpacing)
                .AddNiceText("Количество выбранного типа бруса в 1 м3.", JustifyTextBold, MidLineSpacing)
                .AddNiceText("- Сколько брусьев в 1 м3 понадобится.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество брусьев необходимого для создания конструкции.", JustifyTextBold,
                    MidLineSpacing)
                .AddNiceText("- Общее количество брусьев необходимых для постройки стен по заданным параметрам.",
                    JustifyText, MidLineSpacing)
                .AddNiceText("Объем кубов бруса необходимого для создания конструкции.", JustifyTextBold,
                    MidLineSpacing)
                .AddNiceText("- Необходимый объём кубов бруса.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость кубов бруса.", JustifyTextBold, MidLineSpacing)
                .AddNiceText("- Стоимость кубов бруса в рублях.", JustifyText, MidLineSpacing)
                .AddNiceText("Количество рядов бруса.", JustifyTextBold, MidLineSpacing)
                .AddNiceText("- Необходимое количество рядов бруса.", JustifyText, MidLineSpacing)
                .AddNiceText("Общий вес конструкции.", JustifyTextBold, MidLineSpacing)
                .AddNiceText("- Общий вес всей конструкции.", JustifyText, MidLineSpacing)
                .AddNiceText("Стоимость найма рабочих.", JustifyTextBold, MidLineSpacing)
                .AddNiceText("- Стоимость найма всех работников, которые будут наняты для работы.", JustifyText,
                    MidLineSpacing);

            #endregion

            return CreateDocument(fileName, builder);
        }

        private int AddInputsTable(BalkCalculationOutput output, Section lastPage, int tableIndex)
        {
            var input = output.Input;
            AddBaseInputsTable(output.Input, output, lastPage, ref tableIndex, 8, 2)
                .FillRowWith("Вид бруса", input.BalkType.GetMaterialDescription(input.BalkType.GetEnumDisplayName())!)
                .FillRowWith("Ширина бруса", $"{input.BalkWidth} мм")
                .FillRowWith("Высота бруса", $"{input.BalkHeight} мм")
                .FillRowWith("Длина бруса", $"{input.BalkLength} м")
                .FillRowWith("Стоимость одного куба", $"{input.Price} руб.")
                .EndNiceTable();
            return tableIndex;
        }

        private int AddResultsTable(BalkCalculationOutput output, Section lastPage, int tableIndex)
        {
            AddBaseResultsTable(output, lastPage, ref tableIndex, 12, 2)
                .FillRowWith("Площадь кладки", $"{output.AreaToCoverSquareM} м²")
                .FillRowWith("Площадь, которой не нужна кладка", output.AreaToNotCoverSquareM > 0 ? $"{output.AreaToNotCoverSquareM} м²" : "Без проемов")
                .FillRowWith("Количество выбранного типа бруса в 1 м3", $"{output.OneCubeBalkAmount} шт.")
                .FillRowWith("Количество брусьев необходимого для создания конструкции", $"{output.TotalMaterialAmount} шт.")
                .FillRowWith("Объем бруса в кубе", $"{output.BalkVolumeCubeM} м3")
                .FillRowWith("Объем кубов бруса необходимого для создания конструкции", $"{output.AreaToCoverCubeM} м3")
                .FillRowWith("Стоимость кубов бруса", $"{output.TotalMaterialPrice} руб.")
                .FillRowWith("Количество рядов бруса", $"{output.BalkRowsAmount} шт.")
                .FillRowWith("Общий вес конструкции", $"{output.ConstructionWeight} кг")
                .FillRowWith("Стоимость найма рабочих", output.AllWorkersPrice > 0 ? $"{output.AllWorkersPrice} руб." : "Без найма")
                .FillRowWith("Итоговая стоимость работ с учетом найма работников и закупки материалов", $"{output.TotalMaterialAndWorkersPrice} руб.")
                .EndNiceTable();
            return tableIndex;
        }
    }
}