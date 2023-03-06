using System;
using System.ComponentModel.DataAnnotations;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    ///     Тип кладки.
    /// </summary>
    public enum MasonryType
    {
        [Display(Name = "Каждый ряд")] PerRow = 0,
        [Display(Name = "Через ряд")] PerOneRow,
        [Display(Name = "Через 2 ряда")] PerTwoRows,
        [Display(Name = "Через 3 ряда")] PerThreeRows,
        [Display(Name = "Через 4 ряда")] PerFourRows
    }
    
    public static class MasonryTypeExtensions
    {
        public static int GetValue(this MasonryType type)
            => type switch
            {
                MasonryType.PerRow => 1,
                MasonryType.PerOneRow => 2,
                MasonryType.PerTwoRows => 3,
                MasonryType.PerThreeRows => 4,
                MasonryType.PerFourRows => 5,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }
}