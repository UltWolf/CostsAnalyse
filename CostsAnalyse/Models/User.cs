using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class User:IdentityUser
    {
        public int Year { get; set; }
        public string Country { get; set; }

    }
}
