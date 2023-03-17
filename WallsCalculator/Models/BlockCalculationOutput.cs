using WallsCalculator.Models.Shared;

namespace WallsCalculator.Models
{
    public class BlockCalculationOutput : CalculationOutput
    {
        /// <summary>
        /// Данные для расчета.
        /// </summary>
        public BlockCalculationInput Input { get; set; }
            
        /// <summary>
        /// Кол-во блоков для кладки одного квадратного метра.
        /// </summary>
        public int OneSquareBlocksAmount { get; set; }
            
        /// <summary>
        /// Площадь кладочной сетки.
        /// </summary>
        public double AreaForMasonryGrid { get; set; }
            
        /// <summary>
        /// Число рядов кладочной сетки.
        /// </summary>ы
        public double MasonryGridRowsAmount { get; set; }
            
        /// <summary>
        /// Число блоков в колонне.
        /// </summary>
        public double ColumnBlocksAmount { get; set; }
        
        /// <summary>
        /// Вес конструкции из блоков.
        /// </summary>
        public double ConstructionWeight { get; set; }
    }
}