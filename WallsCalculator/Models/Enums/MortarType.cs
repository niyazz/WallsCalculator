using System;
using System.ComponentModel.DataAnnotations;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    ///     Толщина раствора.
    /// </summary>
    public enum MortarType
    {
        [Display(Name = "10 мм")] Low = 0,
        [Display(Name = "15 мм")] Mid,
        [Display(Name = "20 мм")] High,
    }
    
    public static class MortarTypeExtensions
    {
        public static double GetValue(this MortarType type)
            => type switch
            {
                MortarType.Low => 10,
                MortarType.Mid => 15,
                MortarType.High => 20,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }
}