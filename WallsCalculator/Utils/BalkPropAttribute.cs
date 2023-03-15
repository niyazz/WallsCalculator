using System;
using System.Globalization;
using System.Linq;

namespace WallsCalculator.Utils
{
    /// <summary>
    /// Свойства кирпича.
    /// </summary>
    public class BalkPropAttribute : Attribute
    {
        /// <summary>
        ///  Плотность.
        /// </summary>
        public double Density { get; set; }

        public BalkPropAttribute(double density)
        {
            Density = density;
        }
    }
    
    public static class BalkPropAttributeExtensions
    {
        public static double GetDensity<T>(this T enm) where T : IConvertible
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
                                .GetCustomAttributes(typeof(BalkPropAttribute), false)
                                .FirstOrDefault() is BalkPropAttribute balk)
                        {
                            return balk.Density;
                        }
                    }
                }
            }

            return 0;
        }
    }
}