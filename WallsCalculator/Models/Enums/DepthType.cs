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
        public static double GetValue(this DepthType type)
            => type switch
            {
                DepthType.Half => 0.5,
                DepthType.One => 1,
                DepthType.OneAndHalf => 1.5,
                DepthType.Double => 2,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }
}