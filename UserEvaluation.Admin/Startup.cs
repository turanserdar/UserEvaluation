using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoposphere.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserEvaluation.Data;
using UserEvaluation.Data.Entities;
using UserEvaluation.Services.Concrete;
using UserEvaluation.Services.Interfaces;


namespace UserEvaluation.Admin
{
    public class Startup
    {
      

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserEvaluationDbContext>(option => {

                option.UseSqlServer("Server=.;Database=UserEvaluation;User Id=sa;Password=123;");
            
            
            });

            services.AddScoped<IRepository<User>, EFRepository<User>>();
            services.AddScoped<IRepository<Role>, EFRepository<Role>>();
            services.AddScoped<IRepository<Evaluation>, EFRepository<Evaluation>>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(option =>
              {
                  option.LoginPath = "/auth/login";
                  option.ExpireTimeSpan = TimeSpan.FromHours(1);
              });


            services.AddControllersWithViews();
      
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

            app.UseAuthentication();

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
