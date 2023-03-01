using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WallsCalculator.Utils.Bindings
{
    /// <summary>
    /// Привязчик чисел с точкой из-за разной культуры.
    /// </summary>
    /// <typeparam name="T">Тип числа.</typeparam>
    public class NumberModelBinderProvider<T> : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();

            IModelBinder? binder = null;
            if(typeof(T) == typeof(double))
                binder = new DoubleNumberBinder(new SimpleTypeModelBinder(typeof(T), loggerFactory));

            if (typeof(T) == typeof(decimal))
                binder = new DecimalNumberBinder(new SimpleTypeModelBinder(typeof(T), loggerFactory));

            return context.Metadata.ModelType == typeof(T) ? binder : null;
        }
    }
}
