using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static WallsCalculator.Utils.Errors;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Данные формы для калькулятора стен из бруса.
    /// </summary>
    public class BalkCalculationInput : BaseCalculationInput
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
        /// Цена бруса.
        /// </summary>
        [DisplayName("Цена бруса (руб.)")]
        public decimal Price { get; set; }
    }
}