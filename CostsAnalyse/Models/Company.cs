using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class Company:IEquatable<Company>
    {
        public Company() { }
        public Company(string name,string url)
        {
            this.Name = name;
            this.URL = url;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL  { get; set; }

        public bool Equals(Company other)
        {
            return this.Name.Equals(other.Name);
        }
    }
}
