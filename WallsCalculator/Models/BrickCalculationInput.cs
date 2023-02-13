using System.ComponentModel.DataAnnotations;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Данные форма для калькулятора кирпичных стен.
    /// </summary>
    public class BrickCalculationInput
    {
        /// <summary>
        /// Вид кирпича.
        /// </summary>
        [Required]
        public BrickType BrickType { get; set; }

        /// <summary>
        /// Общая длина всех стен.
        /// </summary>
        [Required]
        public decimal Perimeter { get; set; }

        /// <summary>
        /// Высота стен по углам.
        /// </summary>
        public decimal AngleHeight { get; set; }

        /// <summary>
        /// Тощина стен.
        /// </summary>
        public decimal Depth { get; set; }

        /// <summary>
        /// Тощина раствора.
        /// </summary>
        public decimal MortarDepth { get; set; } = 10.0m;

        /// <summary>
        /// Цена кирпича.
        /// </summary>
        public decimal Price { get; set; }
    }
}
