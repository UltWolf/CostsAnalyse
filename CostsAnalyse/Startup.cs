﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Abstracts;
using CostsAnalyse.Services.Initializers; 
using CostsAnalyse.Services.ScheduleDriver;
using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CostsAnalyse
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
          
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddIdentity<UserApp, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;

                o.Password.RequireLowercase = false;

                o.Password.RequireUppercase = false;

                o.Password.RequireNonAlphanumeric = false;

                o.Password.RequiredLength = 6;
            })
               .AddEntityFrameworkStores<ApplicationContext>()
               .AddDefaultTokenProviders();
             
            services.AddDbContext<ApplicationContext>(options=> options.UseSqlServer(Configuration.GetConnectionString("StringConnection")));
            services.AddAuthentication().AddCookie(); 
            services.AddServerSideBlazor();
            services.AddScoped<ITask, TaskDriver>();
            services.AddMvc(); 
            var provider = services.BuildServiceProvider();
            //lately cut`s in another class for initialization
            StartsInitialize.Initialize();
            DbInitializer dbInitializer = new DbInitializer();
            dbInitializer.Initialize(provider);
            UserInitialize userInitialize = new UserInitialize();
            userInitialize.Initialize(provider);
            RoleInitializer roleInitializer = new RoleInitializer();
            roleInitializer.Initialize(provider);
            AdminInitialize adminInitialize = new AdminInitialize();
            adminInitialize.Initialize(provider); 
            services.AddHangfire((config) => {
                var options = new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true,
                    QueuePollInterval = TimeSpan.FromHours(10)
                };
                config.UseSqlServerStorage("Server=(localdb)\\mssqllocaldb;Database=CostsAnalyseDB;Trusted_Connection=True;MultipleActiveResultSets=true", options);

            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        { 
            loggerFactory.AddLog4Net( );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            } 
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapBlazorHub();
            });
            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 1 });
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            app.UseHangfireDashboard("/dashboard");
            ScheduleDriver.ScheduleReccuringJob();
             

        }
    }
}
