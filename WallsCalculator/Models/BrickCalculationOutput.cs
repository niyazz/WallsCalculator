using System.ComponentModel;
using WallsCalculator.Models.Shared;

namespace WallsCalculator.Models
{
    namespace WallsCalculator.Models
    {
        /// <summary>
        /// Данные форма для калькулятора кирпичных стен.
        /// </summary>
        public class BrickCalculationOutput : CalculationOutput
        {
            /// <summary>
            /// Данные для расчета.
            /// </summary>
            public BrickCalculationInput Input { get; set; }
            
            /// <summary>
            /// Кол-во кирпича для кладки одного квадратного метра.
            /// </summary>
            public int OneSquareBricksAmount { get; set; }
            
            /// <summary>
            /// Площадь кладочной сетки.
            /// </summary>
            public double AreaForMasonryGrid { get; set; }
            
            /// <summary>
            /// Число рядов кладочной сетки.
            /// </summary>ы
            public double MasonryGridRowsAmount { get; set; }
            
            /// <summary>
            /// Число кирпичей в колонне.
            /// </summary>
            public double ColumnBricksAmount { get; set; }
        }
    }
}