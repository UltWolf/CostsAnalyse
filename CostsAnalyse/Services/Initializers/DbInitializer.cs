using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Initializers.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Initializers
{
    public class DbInitializer:IInitialize
    {
     

        public  void Initialize(IServiceProvider provider)
            {
                using (var serviceScope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    ApplicationContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    dbContext.Database.EnsureCreated(); 
                }
            } 
    }
}
