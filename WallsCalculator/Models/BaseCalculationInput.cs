using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static WallsCalculator.Utils.Errors;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Входные данные расчета.
    /// </summary>
    public class BaseCalculationInput
    {
        /// <summary>
        /// Общая длина всех стен.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Общая длина всех стен (м)")]
        public double Perimeter { get; set; }
        
        /// <summary>
        /// Высота стен по углам.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Высота стен по углам (см)")]
        public double AngleHeight { get; set; }

        /// <summary>
        /// Проёмы в стенах.
        /// </summary>
        public IEnumerable<ApertureInput> Apertures { get; set; }
        
        /// <summary>
        /// Проёмы в стенах.
        /// </summary>
        public IEnumerable<WorkerInput> Workers { get; set; }
    }
}