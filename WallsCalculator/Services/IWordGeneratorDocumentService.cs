using System;
using System.IO;
using System.Linq;
using Spire.Doc;
using Spire.Doc.Documents;
using WallsCalculator.Models;
using WallsCalculator.Models.Enums;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Utils;
using static WallsCalculator.Utils.NiceStyles;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace WallsCalculator.Services
{
    public interface IWordGeneratorDocumentService
    {
        /// <summary>
        /// Сгенерировать Word документ. 
        /// </summary>
        /// <param name="calculatorInput">Входные данные калькулятора.</param>
        /// <param name="fileName">Название выходного файла без расширения.</param>
        HttpFileContent Generate(BrickCalculationInput calculatorInput, string fileName = "Результат расчёта");
    }

    public class WordGeneratorDocumentService : IWordGeneratorDocumentService
    {
        private const string MsWord = "application/msword";

        private readonly IBrickCalculator _calculator;

        public WordGeneratorDocumentService(IBrickCalculator calculator)
        {
            _calculator = calculator;
        }
        
        public HttpFileContent Generate(BrickCalculationInput calculatorInput, string fileName = "Результат расчёта")
        {
            var calculated = _calculator.Calculate(calculatorInput)!;
            var builder = new DocFormatBuilder();
            var input = calculated.Input;
            var tableIndex = 1;
            var lastPage = builder
                .AddStyle(BigHeading, TimesNewRoman, 18, HorizontalAlignment.Center, isBold: true)
                .AddStyle(TableHeading, TimesNewRoman, 14, HorizontalAlignment.Left, isBold: true)
                .AddStyle(CenterText,  TimesNewRoman, 14, HorizontalAlignment.Center, isBold: false)
                .AddStyle(LeftText, TimesNewRoman, 14, HorizontalAlignment.Left, isBold: false)
                .AddPage()
                .AddNiceText($"Расчет кирпичной стены от {DateTime.Now:dd/MM/yyyy}\n", BigHeading, MidLineSpacing);
                
            tableIndex = AddInputsTable(calculated, lastPage, tableIndex, input);
            tableIndex = AddAperturesTable(input.Apertures.ToArray(), lastPage, tableIndex);
            tableIndex = AddWorkersTable(input.Workers.ToArray(), lastPage, tableIndex);
            _ = AddResultsTable(calculated, lastPage, tableIndex);
            
            var document = builder.Build();
            var stream = new MemoryStream();
            document.SaveToStream(stream, FileFormat.Doc);

            return new()
            {
                FileName = fileName.LastIndexOf(".") == -1 ? $"{fileName}.doc" : $"{fileName[..fileName.LastIndexOf('.')]}.doc",
                Content = stream.ToArray(),
                ContentType = MsWord
            };
        }
        
        private static int AddInputsTable(BrickCalculationOutput output, Section lastPage, int tableIndex,
            BrickCalculationInput input)
        {
            lastPage.AddNiceText($"Таблица {tableIndex++}. Общие входные данные, мм \n", TableHeading, LowLineSpacing)
                .AddNiceTable(7, 2)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Вид кирпича", input.BrickType.GetBrickDescription())
                .FillRowWith("Тип кладки", input.MasonryType.GetDisplayName())
                .FillRowWith("Общая длина всех стен", input.Perimeter.ToString())
                .FillRowWith("Высота стен по углам", input.AngleHeight.ToString())
                .FillRowWith("Толщина стен", $"{input.DepthType.GetDisplayName()} ({output.WallDepth})")
                .FillRowWith("Толщина раствора", input.MortarType.GetDisplayName())
                .FillRowWith("Цена кирпича", $"{input.Price} руб.")
                .EndNiceTable();
            return tableIndex;
        }
        
        private static int AddWorkersTable(WorkerInput[] workers, Section lastPage, int tableIndex)
        {
            if (workers.Any())
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
        
        private static int AddAperturesTable(ApertureInput[] apertures, Section lastPage, int tableIndex)
        {
            var doorApertures = apertures.Where(x => x.ApertureType == ApertureType.Door).ToArray();
            if (doorApertures.Any(x => x.Height != 0 && x.Width != 0))
            {
                var table = lastPage
                    .AddNiceText($"\n\nТаблица {tableIndex++}. Дверные проемы в стенах, мм \n", TableHeading, LowLineSpacing)
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
                    .AddNiceText($"\n\nТаблица {tableIndex++}. Оконные проемы в стенах, мм \n", TableHeading, LowLineSpacing)
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
        
        private static int AddResultsTable(BrickCalculationOutput output, Section lastPage, int tableIndex)
        {
            lastPage
                .AddNiceText($"\n\nТаблица {tableIndex++}. Результаты расчета \n", TableHeading, LowLineSpacing)
                .AddNiceTable(11, 2)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Количество выбранного типа кирпича  в 1 кв.м.", $"{output.OneSquareBricksAmount} шт.")
                .FillRowWith("Количестов кирпича необходимого для кладки", $"{output.TotalBricksAmount} шт.")
                .FillRowWith("Количество кирпича в одном столбе", $"{output.ColumnBricksAmount} шт.")
                .FillRowWith("Полная стоимость кирпичей", $"{output.TotalBricksPrice} руб.")
                .FillRowWith("Площадь кладки", $"{output.AreaToCover} кв.м.")
                .FillRowWith("Площадь, которой не нужна кладка", $"{output.AreaToNotCover} кв.м.")
                .FillRowWith("Площадь кладочной сетки", $"{output.AreaForMasonryGrid} кв.м.")
                .FillRowWith("Количество рядов кладочной сетки", $"{output.MasonryGridRowsAmount} шт.")
                .FillRowWith("Итоговая цена без найма строителей", $"{output.TotalBricksPrice} руб.")
                .FillRowWith("Итоговая цена найма строителей", $"{output.AllWorkersPrice} руб.")
                .FillRowWith("Итоговая цена с наймом строителей", $"{output.TotalBricksPrice + output.AllWorkersPrice} руб.")
                .EndNiceTable();
            return tableIndex;
        }

    }
}