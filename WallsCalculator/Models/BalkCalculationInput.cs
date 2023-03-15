using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WallsCalculator.Models.Enums;
using static WallsCalculator.Utils.Errors;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Данные формы для калькулятора стен из бруса.
    /// </summary>
    public class BalkCalculationInput : CalculationInput
    {
        /// <summary>
        /// Ширина бруса.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Ширина бруса (мм)")]
        public double BalkWidth { get; set; }
        
        /// <summary>
        /// Высота бруса.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Высота бруса (мм)")]
        public double BalkHeight { get; set; }
        
        /// <summary>
        /// Длина бруса.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Длина бруса (м)")]
        public double BalkLength { get; set; }
        
        /// <summary>
        /// Вид бруса.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Вид бруса")]
        public BalkType BalkType { get; set; }
        
        /// <summary>
        /// Цена за куб бруса.
        /// </summary>
        [DisplayName("Цена за куб (руб.)")]
        public decimal Price { get; set; }
    }
}