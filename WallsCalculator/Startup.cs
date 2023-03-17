using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WallsCalculator.Models;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Services;
using WallsCalculator.Services.Abstractions;
using WallsCalculator.Services.Calculators;
using WallsCalculator.Services.WordGenerators;
using WallsCalculator.Utils;

namespace WallsCalculator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; } 
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                // Use that section if you don`t want to rewrite Jquery validation issue
                // more info here: https://stackoverflow.com/a/48187581
                //   options.ModelBinderProviders.Insert(0, new NumberModelBinderProvider<double>());
                //   options.ModelBinderProviders.Insert(1, new NumberModelBinderProvider<decimal>());
            });
            
            services.AddTransient<ICalculator<BrickCalculationInput, BrickCalculationOutput>, BrickCalculator>();
            services.AddTransient<ICalculator<BalkCalculationInput, BalkCalculationOutput>, BalkCalculator>();
            services.AddTransient<ICalculator<BlockCalculationInput, BlockCalculationOutput>, BlockCalculator>();
            services.AddTransient<IWordGeneratorDocumentService<BlockCalculationInput>, BlockWordGeneratorService>();
            services.AddTransient<IWordGeneratorDocumentService<BrickCalculationInput>, BrickWordGeneratorService>();
            services.AddTransient<IWordGeneratorDocumentService<BalkCalculationInput>, BalkWordGeneratorService>();
            services.AddSingleton(Configuration.GetSection("Editable").Get<EditableSettings>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
