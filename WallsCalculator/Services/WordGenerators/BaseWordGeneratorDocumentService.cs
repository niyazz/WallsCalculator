using System.Linq;
using Spire.Doc;
using Spire.Doc.Documents;
using WallsCalculator.Models;
using WallsCalculator.Models.Enums;
using WallsCalculator.Models.Shared;
using static WallsCalculator.Utils.NiceStyles;
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace WallsCalculator.Services.WordGenerators
{
    public class BaseWordGeneratorDocumentService
    {
        protected const string MsWord = "application/msword";
        
        protected Section AddPage(DocumentFormatBuilder builder)
        {
            return builder
                .AddStyle(BigHeading, TimesNewRoman, 18, HorizontalAlignment.Center, isBold: true)
                .AddStyle(TableHeading, TimesNewRoman, 14, HorizontalAlignment.Left, isBold: true)
                .AddStyle(CenterText, TimesNewRoman, 14, HorizontalAlignment.Center, isBold: false)
                .AddStyle(LeftText, TimesNewRoman, 14, HorizontalAlignment.Left, isBold: false)
                .AddPage();
        }

        protected Table AddBaseInputsTable(CalculationInput input, CalculationOutput output, Section lastPage,
            ref int tableIndex, int rows, int cols)
        {
            return lastPage.AddNiceText($"Таблица {tableIndex++}. Общие входные данные \n", TableHeading,
                    LowLineSpacing)
                .AddNiceTable(rows, cols)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Общая длина всех стен", $"{input.Perimeter} м")
                .FillRowWith("Высота стен по углам", $"{input.AngleHeight} см")
                .FillRowWith("Толщина стены", $"{output.WallDepthCentimeters} см");
        }
        protected int AddAperturesTable(ApertureInput[] apertures, Section lastPage, ref int tableIndex)
        {
            var doorApertures = apertures.Where(x => x.ApertureType == ApertureType.Door).ToArray();
            if (doorApertures.Any(x => x.Height != 0 && x.Width != 0))
            {
                var table = lastPage
                    .AddNiceText($"\n\nТаблица {tableIndex++}. Дверные проемы в стенах, мм \n", TableHeading,
                        LowLineSpacing)
                    .AddNiceTable(doorApertures.Length + 1, 3)
                    .SetNiceTableStyle(CenterText, HighLineSpacing)
                    .FillRowWith("Номер проема", "Ширина", "Высота");

                for (int i = 0; i < doorApertures.Length; i++)
                {
                    table.FillRowWith((i + 1).ToString(), doorApertures[i].Width.ToString(),
                        doorApertures[i].Height.ToString());
                }

                table.EndNiceTable();
            }

            var windowsApertures = apertures.Where(x => x.ApertureType == ApertureType.Window).ToArray();
            if (windowsApertures.Any(x => x.Height != 0 && x.Width != 0))
            {
                var table = lastPage
                    .AddNiceText($"\n\nТаблица {tableIndex++}. Оконные проемы в стенах, мм \n", TableHeading,
                        LowLineSpacing)
                    .AddNiceTable(windowsApertures.Length + 1, 3)
                    .SetNiceTableStyle(CenterText, HighLineSpacing)
                    .FillRowWith("Номер проема", "Ширина", "Высота");

                for (int i = 0; i < windowsApertures.Length; i++)
                {
                    table.FillRowWith((i + 1).ToString(), windowsApertures[i].Width.ToString(),
                        windowsApertures[i].Height.ToString());
                }

                table.EndNiceTable();
            }

            return tableIndex;
        }
        
        protected int AddWorkersTable(WorkerInput[] workers, Section lastPage, ref int tableIndex)
        {
            if (workers.Any(x => x.DurationInDays != 0))
            {
                var table = lastPage
                    .AddNiceText($"\n\nТаблица {tableIndex++}. Наемные рабочие \n", TableHeading, LowLineSpacing)
                    .AddNiceTable(workers.Length + 1, 4)
                    .SetNiceTableStyle(CenterText, HighLineSpacing)
                    .FillRowWith("Номер типа рабочего", "Оплата труда в день", "Число рабочих",
                        "Продолжительность в днях");

                for (int i = 0; i < workers.Length; i++)
                {
                    table.FillRowWith((i + 1).ToString(), workers[i].Price.ToString(),
                        workers[i].QuantityOfWorkers.ToString(), workers[i].DurationInDays.ToString());
                }

                table.EndNiceTable();
            }

            return tableIndex;
        }

        protected Table AddBaseResultsTable(CalculationOutput output, Section lastPage, 
            ref int tableIndex, int rows, int cols)
        {
            return lastPage
                .AddNiceText($"\n\nТаблица {tableIndex++}. Результаты расчета \n", TableHeading, LowLineSpacing)
                .AddNiceTable(rows, cols)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Общая площадь", $"{output.TotalArea} кв.м.")
                .FillRowWith("Площадь возведения стен", $"{output.AreaToCoverSquareM} кв.м.")
                .FillRowWith("Площадь проемов", output.AreaToNotCoverSquareM > 0 ? $"{output.AreaToNotCoverSquareM} кв.м." : "Без проемов")
                .FillRowWith("Необходимое количество материала", $"{output.TotalMaterialAmount} шт.")
                .FillRowWith("Стоимость материалов для возведения стен", $"{output.TotalMaterialPrice} руб.")
                .FillRowWith("Стоимость найма всех работников", output.AllWorkersPrice > 0 ? $"{output.AllWorkersPrice} руб." : "Без найма")
                .FillRowWith("Итоговая стоимость работ с учетом найма работников и закупки материалов", $"{output.TotalMaterialAndWorkersPrice} руб.");
        }
    }
}