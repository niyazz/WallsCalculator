using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace WallsCalculator.Utils
{
    public static class DomainExtensions
    {
        public static string GetDisplayName<T>(this T enm) where T : IConvertible
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
                                .GetCustomAttributes(typeof(DisplayAttribute), false)
                                .FirstOrDefault() is DisplayAttribute display)
                        {
                            return display.Name!;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}