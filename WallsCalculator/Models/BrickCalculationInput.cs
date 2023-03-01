﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WallsCalculator.Models.Enums;
using static WallsCalculator.Utils.Errors;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Данные формы для калькулятора кирпичных стен.
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
        /// Толщина стен.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Толщина стен")]
        public DepthType DepthType { get; set; }

        /// <summary>
        /// Толщина раствора.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Толщина раствора")]
        public MortarType MortarType { get; set; }

        /// <summary>
        /// Цена кирпича.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Цена кирпича (руб.)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Проёмы в стенах.
        /// </summary>
        public IEnumerable<ApertureInput> Apertures { get; set; }
    }
}
