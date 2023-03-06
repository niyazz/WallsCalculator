using System;
using System.IO;
using System.Linq;
using Spire.Doc;
using Spire.Doc.Documents;
using WallsCalculator.Models.Enums;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Utils;
using static WallsCalculator.Utils.NiceStyles;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace WallsCalculator.Services
{
    public interface IWordGeneratorDocumentService
    {
        (string, byte[], string) Generate(BrickCalculationOutput calculationOutput);
    }

    public class WordGeneratorDocumentService : IWordGeneratorDocumentService
    {
        public (string, byte[], string) Generate(BrickCalculationOutput calculationOutput)
        {
            var builder = new DocFormatBuilder();
            var calculationInput = calculationOutput.Input;
            var tableIndex = 1;
            var lastPage = builder
                .AddStyle(BigHeading, TimesNewRoman, 18, HorizontalAlignment.Center, isBold: true)
                .AddStyle(TableHeading, TimesNewRoman, 14, HorizontalAlignment.Left, isBold: true)
                .AddStyle(CenterText,  TimesNewRoman, 14, HorizontalAlignment.Center, isBold: false)
                .AddStyle(LeftText, TimesNewRoman, 14, HorizontalAlignment.Left, isBold: false)
                .AddPage()
                .AddNiceText($"Расчет кирпичной стены от {DateTime.Now:dd/MM/yyyy}\n", BigHeading, MidLineSpacing);
                
            lastPage.AddNiceText($"Таблица {tableIndex++}. Общие входные данные, мм \n", TableHeading, LowLineSpacing)
                .AddNiceTable(7, 2)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Вид кирпича", calculationInput.BrickType.GetBrickDescription())
                .FillRowWith("Тип кладки", calculationInput.MasonryType.GetDisplayName())
                .FillRowWith("Общая длина всех стен", calculationInput.Perimeter.ToString())
                .FillRowWith("Высота стен по углам", calculationInput.AngleHeight.ToString())
                .FillRowWith("Толщина стен", calculationInput.DepthType.GetDisplayName())
                .FillRowWith("Толщина раствора", calculationInput.MortarType.GetDisplayName())
                .FillRowWith("Цена кирпича", $"{calculationInput.Price} руб.")
                .EndNiceTable();

            var doorApertures = calculationInput.Apertures.Where(x => x.ApertureType == ApertureType.Door).ToArray();
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
            
            var windowsApertures = calculationInput.Apertures.Where(x => x.ApertureType == ApertureType.Window).ToArray();
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
            var workers = calculationInput.Workers.ToArray();
            if (workers.Any())
            {
                lastPage
                    .AddNiceText($"\n\nТаблица {tableIndex++}. Наемные рабочие \n", TableHeading, LowLineSpacing)
                    .AddNiceTable(workers.Length + 1, 3)
                    .SetNiceTableStyle(CenterText, HighLineSpacing);
            }

            lastPage
                .AddNiceText($"\n\nТаблица {tableIndex++}. Результаты расчета \n", TableHeading, LowLineSpacing)
                .AddNiceTable(3, 2)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Площадь кладки", $"{calculationOutput.AreaToCover} кв.м.")
                .FillRowWith("Количество кирпича для кладки",  $"{calculationOutput.BricksAmount} шт.")
                .FillRowWith("Цена всей кладки", $"{calculationOutput.AllBricksPrice} руб.")
                .EndNiceTable();
            
            var document = builder.Build();
            var stream = new MemoryStream();
            document.SaveToStream(stream, FileFormat.Doc);
            return ("Расчет.doc", stream.ToArray(), "application/msword");
        }
    }
}