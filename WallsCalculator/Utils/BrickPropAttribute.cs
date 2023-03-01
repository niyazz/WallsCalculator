using System;
using System.Globalization;
using System.Linq;

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
        /// Глубина
        /// </summary>
        public double Depth { get; set; }

        public BrickPropAttribute(string name, double width, double height, double depth)
        {
            Name = name;
            Width = width;
            Height = height;
            Depth = depth;
        }
    }
    
    public static class BrickPropAttributeExtensions
    {
        public static (double, double, double)? GetBrickSizes<T>(this T enm) where T : IConvertible
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
                            return (brick.Width, brick.Height, 
                                brick.Depth);
                        }
                    }
                }
            }

            return null;
        }
        
        public static double? GetBrickVolume<T>(this T enm) where T : IConvertible
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
                            return brick.Width * brick.Height * brick.Depth;
                        }
                    }
                }
            }

            return null;
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
                            return $"'{brick.Name}' ({brick.Width}*{brick.Height}*{brick.Depth})";
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}