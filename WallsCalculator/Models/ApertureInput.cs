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
        [DisplayName("Тип проема")]
        public ApertureType ApertureType { get; set; }
        
        /// <summary>
        /// Ширина проема.
        /// </summary>
        [DisplayName("Ширина проема (мм)")]
        public double Width { get; set; }

        /// <summary>
        /// Высота проема.
        /// </summary>
        [DisplayName("Высота проема (мм)")]
        public double Height { get; set; }
    }
}
