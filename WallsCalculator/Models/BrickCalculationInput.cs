using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WallsCalculator.Utils;
using static WallsCalculator.Utils.ErrorsConstants;

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
        [Required(ErrorMessage = Required)]
        [DisplayName("Вид кирпича")]
        public BrickType BrickType { get; set; }

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
        /// Тощина стен.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Тощина стен (мм)")]
        public double Depth { get; set; }

        /// <summary>
        /// Тощина раствора.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Тощина раствора (мм)")]
        public double MortarDepth { get; set; }

        /// <summary>
        /// Цена кирпича.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Цена кирпича (руб.)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Проёмы в стенах.
        /// </summary>
        public IEnumerable<Aperture> Apertures { get; set; }
    }
}
