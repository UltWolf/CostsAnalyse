using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class Price
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
