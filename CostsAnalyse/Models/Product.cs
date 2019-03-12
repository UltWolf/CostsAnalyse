using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class Product
    {
        public Product()
        {
            this.Information = new List<Information>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public ICollection<Information> Information { get; set; }
        public Price Price { get; set; }
        public Company Company { get; set; }
        
    }
}
