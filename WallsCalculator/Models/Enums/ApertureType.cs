using System.ComponentModel.DataAnnotations;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    ///     Тип проема.
    /// </summary>
    public enum ApertureType
    {
        [Display(Name = "Дверной")] Door = 0,
        [Display(Name = "Оконный")] Window 
    }
}