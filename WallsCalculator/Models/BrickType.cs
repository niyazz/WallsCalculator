using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WallsCalculator.Models
{
    /// <summary>
    /// Тип кирпича.
    /// </summary>
    public enum BrickType
    {
        [Display(Name = "Облицовочный")]
        Facing = 0,
        [Display(Name = "Одиночный")]
        Single,
        [Display(Name = "Полуторный")]
        OneAndHalf
    }
}
