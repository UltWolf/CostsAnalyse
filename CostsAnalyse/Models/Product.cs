using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public Information Information { get; set; }
        public Price Price { get; set; }
        public Company Company { get; set; }
        
    }
}
