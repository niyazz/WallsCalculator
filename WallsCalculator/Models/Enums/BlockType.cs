using System.ComponentModel.DataAnnotations;
using WallsCalculator.Utils;

namespace WallsCalculator.Models.Enums
{
    /// <summary>
    /// Тип блока.
    /// <remarks>Размеры в мм.</remarks>
    /// </summary>
    public enum BlockType
    {
        [MaterialInfoProp(длина:390, ширина:190, высота:188)]
        [Display(Name = "Керамзитобетонный")]
        Facing = 0,
        [MaterialInfoProp(длина:600, ширина:300, высота:200)]
        [Display(Name = "Газобетонный")]
        OneAndHalf,
        [MaterialInfoProp(длина:600, ширина:200, высота:400)]
        [Display(Name = "Пенобетонный")]
        Double
    }
}
