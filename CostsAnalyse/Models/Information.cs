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
        public List<Value> Value { get; set; }
        public Information()
        {
            this.Value = new List<Value>();
        }
        public Information(string key,Value value)
        {
            this.Key = key;
            this.Value = new List<Value>();
            this.Value.Add(value);
        }
      
    }
}
