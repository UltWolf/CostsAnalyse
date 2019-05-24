using CostsAnalyse.Models;
using CostsAnalyse.Services.Initializers.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Initializers
{
    public class RoleInitializer:IInitialize
    {
        public async  void Initialize(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            IdentityResult roleResult; 
            var roleCheckAdmin = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheckAdmin)
            { 
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            } 
            var roleCheckModerator = await RoleManager.RoleExistsAsync("Moderator");
            if (!roleCheckModerator)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Moderator"));
            }
            var roleCheckUser = await RoleManager.RoleExistsAsync("User");
            if (!roleCheckModerator)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }
             
        }
    }
}
