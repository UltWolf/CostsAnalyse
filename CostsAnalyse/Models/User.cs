using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class UserApp: IdentityUser
    {
        [PersonalData]
        public int Year { get; set; }
        [PersonalData]
        public string Country { get; set; } 
        public List<Product> products { get; set; }

    }
}
