using System.ComponentModel.DataAnnotations;
using WallsCalculator.Utils;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    /// Тип кирпича.
    /// </summary>
    public enum BrickType
    {
        [BrickProp("Облицовочный 1НФ", 250, 120, 65)]
        [Display(Name = "Облицовочный (Одинарный)")]
        Facing = 0,
        [BrickProp("Полуторный 1.5НФ", 250, 120, 88)]
        [Display(Name = "Полуторный")]
        OneAndHalf,
        [BrickProp("Двойной 2НФ", 250, 120, 140)]
        [Display(Name = "Двойной")]
        Double
    }
}
