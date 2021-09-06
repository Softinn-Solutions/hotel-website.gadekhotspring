using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmbunLuxuryVillas.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmbunLuxuryVillas
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<AppConfigurations>(Configuration.GetSection("AppSettings"));
            services.AddHttpClient();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "Promotion Detail",
                    defaults: new { controller = "Promotions", action = "Detail" },
                    pattern: "Promotions/{id}");

                routes.MapControllerRoute(
                    name: "Room Details",
                    pattern: "{controller=Rooms}/{action=Details}/{name?}");

                routes.MapControllerRoute(
                    name: "Attractions",
                    defaults: new { controller = "Attractions", action = "Index" },
                    pattern: "Attractions/{attractionType}");

                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                routes.MapControllerRoute(
                    name: "Blog Post",
                    defaults: new { controller = "Blogs", action = "Detail" },
                    pattern: "p/{titleSlug}");
            });
        }
    }
}