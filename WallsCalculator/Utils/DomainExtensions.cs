using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WallsCalculator.Utils
{
    public static class DomainExtensions
    {
        public static string GetEnumDisplayName<T>(this T enm) where T : IConvertible
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