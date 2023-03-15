using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WallsCalculator.Models.Enums;
using static WallsCalculator.Utils.Errors;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Данные формы для калькулятора кирпичных стен.
    /// </summary>
    public class BrickCalculationInput : CalculationInput
    {
        /// <summary>
        /// Вид кирпича.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Вид кирпича")]
        public BrickType BrickType { get; set; }

        /// <summary>
        /// Толщина стен.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Тип кладки кирпича")]
        public DepthType DepthType { get; set; }

        /// <summary>
        /// Толщина раствора.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Толщина раствора")]
        public MortarType MortarType { get; set; }

        /// <summary>
        /// Тип кладки.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Тип кладки сетки")]
        public MasonryType MasonryType { get; set; }
        
        /// <summary>
        /// Цена кирпича.
        /// </summary>
        [DisplayName("Цена кирпича (руб.)")]
        public decimal Price { get; set; }
    }
}
