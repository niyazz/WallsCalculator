using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static WallsCalculator.Utils.Errors;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Данные формы для калькулятора стен из бруса.
    /// </summary>
    public class BalkCalculationInput : ICalculationInput
    {
        /// <summary>
        /// Общая длина всех стен.
        /// </summary>
        public double Perimeter { get; set; }
        
        /// <summary>
        /// Высота стен по углам.
        /// </summary>
        public double AngleHeight { get; set; }
        
        /// <summary>
        /// Проёмы в стенах.
        /// </summary>
        public IEnumerable<ApertureInput> Apertures { get; set; }
        
        /// <summary>
        /// Цена бруса.
        /// </summary>
        [DisplayName("Цена бруса (руб.)")]
        public decimal Price { get; set; }
        
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
    }
}