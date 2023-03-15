using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WallsCalculator.Models;
using WallsCalculator.Models.Enums;
using WallsCalculator.Models.WallsCalculator.Models;
using WallsCalculator.Services;
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
            var standards = Enum.GetValues(typeof(DepthType))
                .Cast<DepthType>()
                .ToDictionary(dt => dt, 
                    dt => Configuration.GetSection(Enum.GetName(dt)).GetChildren()
                        .ToDictionary(bt => (BrickType)Enum.Parse(typeof(BrickType), bt.Key), bt => int.Parse(bt.Value)));

            services.AddSingleton(new BrickStandardOptions
            {
                Standards = standards
            });
            
            services.AddControllersWithViews(options =>
            {
                // Use that section if you don`t want to rewrite Jquery validation issue
                // more info here: https://stackoverflow.com/a/48187581
                //   options.ModelBinderProviders.Insert(0, new NumberModelBinderProvider<double>());
                //   options.ModelBinderProviders.Insert(1, new NumberModelBinderProvider<decimal>());
            });
            
            services.AddTransient<ICalculator<BrickCalculationInput, BrickCalculationOutput>, BrickCalculator>();
            services.AddTransient<ICalculator<BalkCalculationInput, BalkCalculationOutput>, BalkCalculator>();
            services.AddTransient<IWordGeneratorDocumentService<BrickCalculationInput>, BrickWordGeneratorService>();
            services.AddTransient<IWordGeneratorDocumentService<BalkCalculationInput>, BalkWordGeneratorService>();
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
