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
            /// Кол-во кирпича для кладки.
            /// </summary>
            public long BrickAmount { get; set; }
            
            /// <summary>
            /// Цена за все кирпичи.
            /// </summary>
            public decimal FullPrice { get; set; }
            
            /// <summary>
            /// Квадратных метров.
            /// </summary>
            public double Area { get; set; }
        }
    }
}