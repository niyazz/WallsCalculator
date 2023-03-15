using System.ComponentModel.DataAnnotations;
using WallsCalculator.Utils;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    /// Тип бруса.
    /// </summary>
    public enum BalkType
    {
        [BalkProp(480)]
        [Display(Name = "Сосна 480 кг/м")]
        Pine = 0,
        [BalkProp(420)]
        [Display(Name = "Ель 420 кг/м")]
        Spruce,
        [BalkProp(655)]
        [Display(Name = "Дуб 655 кг/м")]
        Oak
    }
}