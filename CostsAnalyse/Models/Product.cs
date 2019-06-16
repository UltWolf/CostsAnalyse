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
        public bool IsNull(){
            return ((Name =="") && (Index==""));
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string UrlImage{get;set;}
        public string Index{get;set;}
        public  decimal Max{get;set;} = 0;
        public  decimal Min{get;set;} = 0;
        public List<UserProduct> Subscribers{get;set;} = new List<UserProduct>();
        public ICollection<Information> Information { get; set; }
        public List<Price> LastPrice { get; set; }
 
        public List<Price> Price { get; set; }
      
        
    }
}
