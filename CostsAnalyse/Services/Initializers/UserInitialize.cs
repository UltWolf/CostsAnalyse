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

        public void Initialize(IServiceProvider serviceProvider)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<UserApp>>();
            UserApp userApp = new UserApp() { Email = "ultwolf@gmail.com", UserName = "UltWolf" };
            UserManager.CreateAsync(userApp,"1234");

        }
    }
}
