using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartMedical.BLL;
using SmartMedical.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartMedical.BLL.SmartMedicalBLL;

namespace SmartMedical
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddTransient<SmartMedicalBLL>();
            services.AddTransient<DBHelper>();
            services.AddTransient<LoginTel>();
            //1.配置跨域处理，允许所有来源： 
            services.AddCors(options =>
                options.AddPolicy("smartmedical", p => p.AllowAnyOrigin())
            );
            services.AddMemoryCache().AddSimpleCaptcha(builder =>
            {
                builder.UseMemoryStore();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();


            app.UseAuthorization();
            app.UseCors("smartmedical");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
