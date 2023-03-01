using System;
using System.IO;
using System.Linq;
using Spire.Doc;
using Spire.Doc.Documents;
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
                .AddNiceTable(6, 2)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Вид кирпича", calculationInput.BrickType.GetBrickDescription())
                .FillRowWith("Общая длина всех стен", calculationInput.Perimeter.ToString())
                .FillRowWith("Высота стен по углам", calculationInput.AngleHeight.ToString())
                .FillRowWith("Толщина стен", calculationInput.DepthType.GetDisplayName())
                .FillRowWith("Толщина раствора", calculationInput.MortarType.GetDisplayName())
                .FillRowWith("Цена кирпича", $"{calculationInput.Price} руб.")
                .EndNiceTable();

            var apertures = calculationInput.Apertures.ToArray();
            if (apertures.Any(x => x.Height != 0 && x.Width != 0))
            {
                var table = lastPage
                    .AddNiceText($"\n\nТаблица {tableIndex++}. Проемы в стенах, мм \n", TableHeading, LowLineSpacing)
                    .AddNiceTable(calculationInput.Apertures.Count() + 1, 3)
                    .SetNiceTableStyle(CenterText, HighLineSpacing)
                    .FillRowWith("Номер проема", "Ширина", "Высота");

                for (int i = 0; i < apertures.Length; i++)
                {
                    table.FillRowWith((i + 1).ToString(), apertures[i].Width.ToString(),
                        apertures[i].Height.ToString());
                }

                table.EndNiceTable();
            }
            
            lastPage
                .AddNiceText($"\n\nТаблица {tableIndex++}. Результаты расчета \n", TableHeading, LowLineSpacing)
                .AddNiceTable(3, 2)
                .SetNiceTableStyle(LeftText, HighLineSpacing)
                .FillRowWith("Площадь кладки", $"{calculationOutput.Area} кв.м.")
                .FillRowWith("Количество кирпича для кладки",  $"{calculationOutput.BrickAmount} шт.")
                .FillRowWith("Цена всей кладки", $"{calculationOutput.FullPrice} руб.")
                .EndNiceTable();
            
            var document = builder.Build();
            var stream = new MemoryStream();
            document.SaveToStream(stream, FileFormat.Doc);
            return ("Расчет.doc", stream.ToArray(), "application/msword");
        }
    }
}