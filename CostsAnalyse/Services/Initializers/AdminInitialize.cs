using CostsAnalyse.Models;
using CostsAnalyse.Services.Initializers.Basic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Initializers
{
    public class AdminInitialize : IInitialize
    {
        public async void Initialize(IServiceProvider serviceProvider)
        {
           var manager =  serviceProvider.GetRequiredService<UserManager<UserApp>>();
           var admin = await manager.FindByEmailAsync("ultwolf@gmail.com");
            manager.AddToRolesAsync(admin, new List<string>() { "Administrator" });
        }
    }
}
