using System;
using System.Globalization;
using System.Linq;
using WallsCalculator.Models.Enums;
#nullable enable

namespace WallsCalculator.Utils
{
    /// <summary>
    /// Свойства кирпича.
    /// </summary>
    public class MaterialInfoPropAttribute : Attribute
    {
        /// <summary>
        ///  Плотность.
        /// </summary>
        public double? Density { get; }
        
        /// <summary>
        ///  Ширина.
        /// </summary>
        public double Width { get; }
        
        /// <summary>
        ///  Высота.
        /// </summary>
        public double Height { get; }
        
        /// <summary>
        /// Длина.
        /// </summary>
        public double Length { get; }

        public MaterialInfoPropAttribute(double длина, double ширина, double высота)
        {
            Width = ширина;
            Height = высота;
            Length = длина;
        }
        
        public MaterialInfoPropAttribute(double плотность)
        {
            Density = плотность;
        }
    }
    
    public static class MaterialInfoPropAttributeExtensions
    {
        public static (double, double, double) GetMaterialSizes<T>(this T enm) where T : IConvertible
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
                                .GetCustomAttributes(typeof(MaterialInfoPropAttribute), false)
                                .FirstOrDefault() is MaterialInfoPropAttribute materialInfo)
                        {
                            return (materialInfo.Length, materialInfo.Width, materialInfo.Height);
                        }
                    }
                }
            }

            return (0, 0, 0);
        }
        
        public static double? GetMaterialDensity<T>(this T enm) where T : IConvertible
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
                                .GetCustomAttributes(typeof(MaterialInfoPropAttribute), false)
                                .FirstOrDefault() is MaterialInfoPropAttribute {Density: { }} materialInfo)
                            return materialInfo.Density.Value;
                    }
                }
            }

            return null;
        }

        public static string? GetMaterialDescription<T>(this T enm, string name) where T : IConvertible
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
                                .GetCustomAttributes(typeof(MaterialInfoPropAttribute), false)
                                .FirstOrDefault() is MaterialInfoPropAttribute materialInfo)
                        {
                            if (materialInfo.Density.HasValue)
                                return $"{name} ({materialInfo.Density} кг/м)";
                            
                            return $"{name} ({materialInfo.Length}*{materialInfo.Width}*{materialInfo.Height} мм)";
                        }
                    }
                }
            }

            return null;
        }
        
        public static int GetMaterialAmountInSquareMeters(this DepthType depthType, 
            double length, double width, double height, double mortarValue)
        {
            const int oneMeter = 1000;
            double cols, rows;
            switch (depthType)
            {
                case DepthType.Half:
                    cols = oneMeter / (length + mortarValue);
                    rows = oneMeter / (height + mortarValue) ;
                    return Convert.ToInt32(Math.Floor(cols * rows));

                case DepthType.One:
                    cols = oneMeter / (width + mortarValue);
                    rows = oneMeter / (height + mortarValue);
                    return Convert.ToInt32(Math.Floor(cols * rows));
                
                case DepthType.OneAndHalf:
                    var cols2 = oneMeter / (length + mortarValue);
                    cols = oneMeter / (width + mortarValue);
                    rows = oneMeter / (height + mortarValue);
                    return Convert.ToInt32(Math.Floor(cols * rows + rows * cols2));
                
                case DepthType.Double:
                    cols = oneMeter / (width + mortarValue);
                    rows = oneMeter / (height + mortarValue);
                    return Convert.ToInt32(Math.Floor(cols * rows * 2));
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(depthType), depthType, null);
            }
        }
    }
}