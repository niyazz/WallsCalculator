using System.Collections.Generic;

namespace WallsCalculator.Models
{
    namespace WallsCalculator.Models
    {
        /// <summary>
        /// Данные форма для калькулятора кирпичных стен.
        /// </summary>
        public class BrickCalculationOutput
        {
            /// <summary>
            /// Данные для расчета.
            /// </summary>
            public BrickCalculationInput Input { get; set; }
            
            /// <summary>
            /// Кол-во кирпича для кладки в одного квадратного метра.
            /// </summary>
            public int BricksInOneSquareM { get; set; }
            
            /// <summary>
            /// Кол-во кирпича для кладки.
            /// </summary>
            public int BricksAmount { get; set; }
            
            /// <summary>
            /// Цена за все кирпичи.
            /// </summary>
            public decimal AllBricksPrice { get; set; }
            
            /// <summary>
            /// Площадь на кладку.
            /// </summary>
            public double AreaToCover { get; set; }
            
            /// <summary>
            /// Площадь ненуждающаяся в кладке.
            /// </summary>
            public double AreaToNotCover { get; set; }
            
            /// <summary>
            /// Стоимость найма всех работников.
            /// </summary>
            public decimal AllWorkersPrice { get; set; }
        }
    }
}