using System.ComponentModel.DataAnnotations;
using WallsCalculator.Utils;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    /// Тип кирпича.
    /// <remarks>Размеры в мм.</remarks>
    /// </summary>
    public enum BrickType
    {
        [MaterialInfoProp(длина:250, ширина:120, высота:65)]
        [Display(Name = "Облицовочный 1НФ")]
        Facing = 0,
        [MaterialInfoProp(длина:250, ширина:120, высота:88)]
        [Display(Name = "Полуторный 1.5НФ")]
        OneAndHalf,
        [MaterialInfoProp(длина:250, ширина:120, высота:140)]
        [Display(Name = "Двойной 2.1НФ")]
        Double
    }
}
