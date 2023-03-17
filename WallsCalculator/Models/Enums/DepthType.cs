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
        [Display(Name = "Пол изделия")] Half = 0,
        [Display(Name = "1 изделие")] One,
        [Display(Name = "1,5 изделия")] OneAndHalf,
        [Display(Name = "2 изделия")] Double
    }

    public static class DepthTypeExtensions
    {
        public static double GetDepth(this DepthType type, double length, double width, double height, double mortarValue) =>
            type switch
            {
                DepthType.Half => width,
                DepthType.One => length,
                DepthType.OneAndHalf => width + length + mortarValue,
                DepthType.Double => length * 2 + mortarValue,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }
}