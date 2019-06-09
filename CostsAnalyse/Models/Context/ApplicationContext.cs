using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models.Context
{
    public class ApplicationContext:IdentityDbContext<UserApp, IdentityRole, string>
    {


        //public ApplicationContext(DbContextOptions options) : base(options) { }
        public ApplicationContext() { }
       
        public ApplicationContext(DbContextOptions<ApplicationContext> dbContext) : base(dbContext)
        {

        } 
   
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Changes> Changes{get;set;}
        public DbSet<Information> Informations { get; set; } 
        public DbSet<Price> Prices { get; set; } 
    }
}
