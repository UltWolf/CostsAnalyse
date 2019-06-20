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
    public class UserInitialize:IInitialize
    { 

        public async   void Initialize(IServiceProvider serviceProvider)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<UserApp>>();
            var user = await UserManager.FindByEmailAsync("ultwolf@gmail.com");
            if (user == null)
            {
                UserApp userApp = new UserApp() { Email = "ultwolf@gmail.com", UserName = "UltWolf", Country = "Ukraine" };
                await UserManager.CreateAsync(userApp, "123456Vitalka");
            }
        }
    }
}
