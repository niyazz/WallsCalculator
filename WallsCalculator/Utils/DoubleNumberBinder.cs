using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WallsCalculator.Utils
{
    /// <summary>
    /// Конвертирует число с точкой в число с запятой.
    /// </summary>
    public class DoubleNumberBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackBinder;
        public DoubleNumberBinder(IModelBinder modelBinder)
        {
            _fallbackBinder = modelBinder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            var boundValue = bindingContext.ValueProvider.GetValue(modelName);

            if (boundValue == ValueProviderResult.None)
            {
                return _fallbackBinder.BindModelAsync(bindingContext);
            }
            
            var stringNumber = boundValue.FirstValue;

            if (stringNumber != null)
            {
                var wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                var alternateSeperator = (wantedSeperator == "," ? "." : ",");

                if (stringNumber.IndexOf(wantedSeperator, StringComparison.Ordinal) == -1
                && stringNumber.IndexOf(alternateSeperator, StringComparison.Ordinal) != -1)
                {
                    stringNumber = stringNumber.Replace(alternateSeperator, wantedSeperator);
                    var result = double.Parse(stringNumber, NumberStyles.Any);
                    bindingContext.Result = ModelBindingResult.Success(result);
                    return Task.CompletedTask;
                }
            }
            return _fallbackBinder.BindModelAsync(bindingContext);
        }
    }
}
