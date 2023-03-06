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
        [Display(Name = "Облицовочный 1НФ")]
        Facing = 0,
        [BrickProp("Полуторный 1.5НФ", 250, 120, 88)]
        [Display(Name = "Полуторный 1.5НФ")]
        OneAndHalf,
        [BrickProp("Двойной 2.1НФ", 250, 120, 140)]
        [Display(Name = "Двойной 2.1НФ")]
        Double
    }
}
