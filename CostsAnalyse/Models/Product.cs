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
        public void AddInformation(string key, Value value)
        {
            bool IsExist = Information.Any(m => m.Key == key);
            Information information;
            if (IsExist)
            {
                information = Information.First(m => m.Key == key);
                information.Value.Add(value);
            }
            else
            {
                information = new Information(key, value);
            }
            this.Information.Add(information); 
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public ICollection<Information> Information { get; set; }
        public Price Price { get; set; }
        public Company Company { get; set; }
        
    }
}
