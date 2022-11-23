using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskFour.DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using TaskFour.Models;

namespace TaskFour
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
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddDbContext<UserDbContext>(option =>      
            option.UseSqlServer(Configuration.GetConnectionString("UserDb")));

            services.AddDefaultIdentity<User>().AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<UserDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            //services.AddMvc().AddXmlSerializerFormatters();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = "/User/Login";
            //    });

            //services.ConfigureApplicationCookie(config =>
            //{
            //    config.LogoutPath = "/User/Login";
            //});

            // logout when deleted
            //services.Configure<SecurityStampValidatorOptions>(options =>
            //{
            //    options.ValidationInterval = TimeSpan.FromMinutes(1);
            //});
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}
