using System.IO;
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
        
        protected static HttpFileContent CreateDocument(string fileName, DocumentFormatBuilder builder)
        {
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
        
        protected Section AddPage(DocumentFormatBuilder builder, bool addStyle = true)
        {
            if (addStyle)
            {
                return builder
                    .AddStyle(BigHeading, TimesNewRoman, 18, HorizontalAlignment.Center, isBold: true)
                    .AddStyle(TableHeading, TimesNewRoman, 14, HorizontalAlignment.Left, isBold: true)
                    .AddStyle(CenterText, TimesNewRoman, 14, HorizontalAlignment.Center, isBold: false)
                    .AddStyle(LeftText, TimesNewRoman, 14, HorizontalAlignment.Left, isBold: false)
                    .AddStyle(JustifyText, TimesNewRoman, 12, HorizontalAlignment.Justify, isBold: false)
                    .AddStyle(JustifyTextBold, TimesNewRoman, 12, HorizontalAlignment.Justify, isBold: true)
                    .AddPage();
            }
            
            return builder
                .AddPage();
        }

        protected Table AddBaseInputsTable(CalculationInput input, CalculationOutput output, Section lastPage,
            ref int tableIndex, int rows, int cols)
        {
            return lastPage.AddNiceText($"\nТаблица {tableIndex++}. Общие входные данные", TableHeading, MidLineSpacing)
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
                    .AddNiceText($"\nТаблица {tableIndex++}. Дверные проемы в стенах, мм", TableHeading, MidLineSpacing)
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
                    .AddNiceText($"\nТаблица {tableIndex++}. Оконные проемы в стенах, мм", TableHeading, MidLineSpacing)
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
                    .AddNiceText($"\nТаблица {tableIndex++}. Наемные рабочие", TableHeading, MidLineSpacing)
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
                .AddNiceText($"\nТаблица {tableIndex++}. Результаты расчета", TableHeading, MidLineSpacing)
                .AddNiceTable(rows, cols)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Общая площадь", $"{output.TotalArea} м²");
        }
    }
}