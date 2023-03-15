using System;
using System.Globalization;
using System.Linq;
using WallsCalculator.Models.Enums;

namespace WallsCalculator.Utils
{
    /// <summary>
    /// Свойства кирпича.
    /// </summary>
    public class BrickPropAttribute : Attribute
    {
        /// <summary>
        ///  Название.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///  Ширина.
        /// </summary>
        public double Width { get; set; }
        
        /// <summary>
        ///  Высота.
        /// </summary>
        public double Height { get; set; }
        
        /// <summary>
        /// Длина.
        /// </summary>
        public double Length { get; set; }

        public BrickPropAttribute(string name, double length, double width, double height)
        {
            Name = name;
            Width = width;
            Height = height;
            Length = length;
        }
    }
    
    public static class BrickPropAttributeExtensions
    {
        public static (double, double, double) GetBrickSizes<T>(this T enm) where T : IConvertible
        {
            if (enm is Enum)
            {
                var type = enm.GetType();
                var enumValues = Enum.GetValues(type);

                foreach (int val in enumValues)
                {
                    if (val == enm.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val)!);

                        if (memInfo[0]
                                .GetCustomAttributes(typeof(BrickPropAttribute), false)
                                .FirstOrDefault() is BrickPropAttribute brick)
                        {
                            return (brick.Length, brick.Width, brick.Height);
                        }
                    }
                }
            }

            return (0, 0, 0);
        }

        public static string GetBrickDescription<T>(this T enm) where T : IConvertible
        {
            if (enm is Enum)
            {
                var type = enm.GetType();
                var enumValues = Enum.GetValues(type);

                foreach (int val in enumValues)
                {
                    if (val == enm.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val)!);

                        if (memInfo[0]
                                .GetCustomAttributes(typeof(BrickPropAttribute), false)
                                .FirstOrDefault() is BrickPropAttribute brick)
                        {
                            return $"{brick.Name} ({brick.Length}*{brick.Width}*{brick.Height})";
                        }
                    }
                }
            }

            return string.Empty;
        }
        
        public static int GetBricksAmountInSquareMeters(this BrickType brickType, DepthType depthType, double mortarValue)
        {
            const int oneMeter = 1000;
            double l, w, h, cols, rows;
            switch (depthType)
            {
                case DepthType.Half:
                    (l, _, h) = brickType.GetBrickSizes();
                    cols = oneMeter / (l + mortarValue);
                    rows = oneMeter / (h + mortarValue) ;
                    return Convert.ToInt32(Math.Floor(cols * rows));

                case DepthType.One:
                    (_, w, h) = brickType.GetBrickSizes();
                    cols = oneMeter / (w + mortarValue);
                    rows = oneMeter / (h + mortarValue);
                    return Convert.ToInt32(Math.Floor(cols * rows));
                
                case DepthType.OneAndHalf:
                    (l, w, h) = brickType.GetBrickSizes();
                    var cols2 = oneMeter / (l + mortarValue);
                    cols = oneMeter / (w + mortarValue);
                    rows = oneMeter / (h + mortarValue);
                    return Convert.ToInt32(Math.Floor(cols * rows + rows * cols2));
                
                case DepthType.Double:
                    (_, w, h) = brickType.GetBrickSizes();
                    cols = oneMeter / (w + mortarValue);
                    rows = oneMeter / (h + mortarValue);
                    return Convert.ToInt32(Math.Floor(cols * rows * 2));
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(depthType), depthType, null);
            }
        }
    }
}