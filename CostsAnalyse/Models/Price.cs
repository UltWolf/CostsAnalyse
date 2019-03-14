using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class Price
    {
        public int Id { get; set; }
        public Price() { }
        public Price(decimal cost ) {
            this.Cost = cost;
            this.Date = DateTime.Now;
        }
        public Price(decimal cost, DateTime date)
        {
            this.Cost = cost; 
            this.Date = date;
        }
        public Price(decimal cost,  decimal oldCost)
        {
            this.Cost = cost;
            this.Date = DateTime.Now;
            this.IsDiscont = true;
            this.OldCost = oldCost;
        }
        public Price(decimal cost, DateTime date,decimal oldCost)
        {
            this.Cost = cost;
            this.Date = date;
            this.IsDiscont = true;
            this.OldCost = oldCost;
        }
        public decimal Cost { get; set; }
        public bool IsDiscont { get; set; }
        public decimal OldCost { get; set; }
        public DateTime Date { get; set; }
    }
}
