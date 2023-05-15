using System.ComponentModel;
using WallsCalculator.Models.Enums;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Проем.
    /// </summary>
    public class ApertureInput
    {
        /// <summary>
        /// Тип проема.
        /// </summary>
        [DisplayName("Тип проёма")]
        public ApertureType ApertureType { get; set; }
        
        /// <summary>
        /// Ширина проема.
        /// </summary>
        [DisplayName("Ширина проёма (мм)")]
        public double Width { get; set; }

        /// <summary>
        /// Высота проема.
        /// </summary>
        [DisplayName("Высота проёма (мм)")]
        public double Height { get; set; }
        
        /// <summary>
        /// Число таких проёмов.
        /// </summary>
        [DisplayName("Количество")]
        public double Quantity { get; set; }
    }
}
