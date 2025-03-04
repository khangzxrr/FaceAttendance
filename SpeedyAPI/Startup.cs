using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpeedyAPI.Data;
using System;

namespace SpeedyAPI
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
            services.AddHttpClient();

            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });

            services.AddHttpContextAccessor(); //allow use httpcontext in view


            services.AddDbContext<DBAttendanceContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("speedy")));

            services.AddDbContext<DBSubjectContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("speedy")));

            services.AddDbContext<DBTeacherContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("speedy")));

            services.AddDbContext<DBStudentContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("speedy")));

            services.AddDbContext<DBMajorContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("speedy")));

            services.AddDbContext<DBKeyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("speedy")));

            services.AddDbContext<DBSchoolLoginContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("speedy")));
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

            app.UseSession();

            app.UseRouting();

            app.UseCors();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "webcamattendance",
                    pattern: "{controller=WebcamAttendance}/{action=Index}");
            });
            
        }
    }
}
