using System;
using System.ComponentModel.DataAnnotations;
using WallsCalculator.Utils;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    ///     Толщина кладки.
    /// </summary>
    public enum DepthType
    {
        [Display(Name = "Пол кирпича")] Half = 0,
        [Display(Name = "1 кирпич")] One,
        [Display(Name = "1,5 кирпича")] OneAndHalf,
        [Display(Name = "2 кирпича")] Double
    }

    public static class DepthTypeExtensions
    {
        public static double GetDepth(this DepthType type, BrickType brickType, MortarType mortarType) =>
            type switch
            {
                DepthType.Half => brickType.GetBrickSizes().Item2,
                DepthType.One => brickType.GetBrickSizes().Item1,
                DepthType.OneAndHalf => brickType.GetBrickSizes().Item1 + brickType.GetBrickSizes().Item2 + mortarType.GetValue(),
                DepthType.Double => brickType.GetBrickSizes().Item1 * 2 + mortarType.GetValue(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }
}