using Training.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Training.Models.V2.Domain.Interface;
using Training.Models.V2.Domain;
using Training.Models.V2.Repositories.Interface;
using Training.Models.V2.Repositories;

namespace Training
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //// //Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson 
            //services.AddControllers().AddNewtonsoftJson();
            //services.AddControllersWithViews().AddNewtonsoftJson();
            //services.AddRazorPages().AddNewtonsoftJson();

            services.AddControllersWithViews();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true; // 忽略Null值
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // 物件命名依照原本命名輸出
                options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter()); // 格式化日期时间格式
            });

            services.AddSingleton<IGetMaskDataDomain, GetMaskDataDomain>();
            services.AddSingleton<IGetMaskDataRepository, GetMaskDataRepository>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
