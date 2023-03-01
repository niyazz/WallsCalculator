using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static WallsCalculator.Utils.Errors;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Входные данные расчета.
    /// </summary>
    public interface ICalculationInput
    {
        /// <summary>
        /// Общая длина всех стен.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Общая длина всех стен (мм)")]
        public double Perimeter { get; set; }
        
        /// <summary>
        /// Высота стен по углам.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Высота стен по углам (мм)")]
        public double AngleHeight { get; set; }
        
        /// <summary>
        /// Цена изделия.
        /// </summary>
        [Required(ErrorMessage = Required)]
        public decimal Price { get; set; }
        
        /// <summary>
        /// Проёмы в стенах.
        /// </summary>
        public IEnumerable<ApertureInput> Apertures { get; set; }
    }
}