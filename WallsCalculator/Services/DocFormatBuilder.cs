using Spire.Doc;
using Spire.Doc.Documents;

namespace WallsCalculator.Services
{
    /// <summary>
    /// Построитель документа формата doc.
    /// </summary>
    public class DocFormatBuilder
    {
        private readonly Document _document;
        
        public DocFormatBuilder()
        {
            _document = new Document();
        }

        public Section AddPage(float top = 56.7f, float bottom = 56.7f, float left = 85.1f, float right = 42.56f)
        {
            var page = _document.AddSection();
            page.PageSetup.Margins.Bottom = bottom;
            page.PageSetup.Margins.Top = top;
            page.PageSetup.Margins.Left = left;
            page.PageSetup.Margins.Right = right;

            return page;
        }

        public DocFormatBuilder AddStyle(string styleName, string fontName, float fontSize, HorizontalAlignment hAlignment, bool isBold)
        {
            var style = new ParagraphStyle(_document)
            {
                Name = styleName,
                ParagraphFormat ={HorizontalAlignment = hAlignment},
                CharacterFormat =
                {
                    FontName = fontName,
                    FontSize = fontSize,
                    Bold = isBold
                }
            };
            
            _document.Styles.Add(style);

            return this;
        }
        
        public Document Build() => _document;
    }

    /// <summary>
    /// Построитель страницы.
    /// </summary>
    public static class PageBuilder
    {
        public static Section AddNiceText(this Section page, string text, string styleName, float lineSpacing)
        {
           var heading = page.AddParagraph();
           heading.Format.LineSpacing = lineSpacing;
           heading.AppendText(text);
           heading.ApplyStyle(styleName);

           return page;
        }
        
        public static Table AddNiceTable(this Section page, int rowsCount, int columnsCount)
        {
            var table = page.AddTable(true);
            table.ResetCells(rowsCount, columnsCount);

            return table;
        }
    }
    
    /// <summary>
    /// Построитель таблицы.
    /// </summary>
    public static class TableBuilder
    {
        private static int _currentRow;
        private static float _lineSpacing;
        private static string? _styleName;
        
        public static Table FillRowWith(this Table table, params string[] values)
        {
            for (var i = 0; i < table.Rows[_currentRow].Cells.Count; i++)
            {
                var cellText = table.Rows[_currentRow].Cells[i].AddParagraph();
                cellText.Format.LineSpacing = _lineSpacing;
                cellText.AppendText(values[i]);
                cellText.ApplyStyle(_styleName);
            }
            _currentRow++;
            return table;
        }
        
        public static Table SetNiceTableStyle(this Table table, string styleName, float lineSpacing)
        {
            _styleName = styleName;
            _lineSpacing = lineSpacing;
            return table;
        }
        
        public static void EndNiceTable(this Table table) => _currentRow = 0;
    }
    
}