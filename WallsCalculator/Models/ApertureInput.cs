using System.ComponentModel;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Проём.
    /// </summary>
    public class ApertureInput
    {
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
