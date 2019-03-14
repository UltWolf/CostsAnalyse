using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class Information
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public List<string> Value { get; set; }
        public Information()
        {
            this.Value = new List<string>();
        }
        public Information(string key,string value)
        {
            this.Key = key;
            this.Value = new List<string>();
            this.Value.Add(value);
        }
      
    }
}
