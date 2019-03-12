using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models.Context
{
    public class UserContext:DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
