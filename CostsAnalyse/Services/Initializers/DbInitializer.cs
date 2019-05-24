using CostsAnalyse.Models.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Initializers
{
    public class DbInitializer
    { 

            public static void InitializeMigrations(IApplicationBuilder app)
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    ApplicationContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    dbContext.Database.EnsureCreated(); 
                }
            } 
    }
}
