using System.ComponentModel.DataAnnotations;
using WallsCalculator.Utils;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    /// Тип бруса.
    /// <remarks>Плотность в кг/м.</remarks>
    /// </summary>
    public enum BalkType
    {
        [MaterialInfoProp(плотность:480)]
        [Display(Name = "Сосна")]
        Pine = 0,
        [MaterialInfoProp(плотность:420)]
        [Display(Name = "Ель")]
        Spruce,
        [MaterialInfoProp(плотность:655)]
        [Display(Name = "Дуб")]
        Oak
    }
}