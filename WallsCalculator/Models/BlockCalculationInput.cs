using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WallsCalculator.Models.Enums;
using static WallsCalculator.Utils.Errors;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Данные формы для калькулятора блочных стен.
    /// </summary>
    public class BlockCalculationInput : CalculationInput
    {
        /// <summary>
        /// Вид блока.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Вид блока")]
        public BlockType BlockType { get; set; }

        /// <summary>
        /// Толщина стен.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Тип кладки блока")]
        public DepthType DepthType { get; set; }

        /// <summary>
        /// Толщина раствора.
        /// </summary>
        [DisplayName("Толщина раствора (мм)")]
        public double MortarValue { get; set; }

        /// <summary>
        /// Тип кладки.
        /// </summary>
        [Required(ErrorMessage = Required)]
        [DisplayName("Тип кладки сетки")]
        public MasonryType MasonryType { get; set; }
        
        /// <summary>
        /// Вес блока.
        /// </summary>
        [DisplayName("Вес блока (кг)")]
        public double BlockWeight { get; set; }
        
        /// <summary>
        /// Цена блока.
        /// </summary>
        [DisplayName("Цена блока (руб.)")]
        public decimal Price { get; set; }  
    }
}