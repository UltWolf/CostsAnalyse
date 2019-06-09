using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Price(decimal cost,Company company ) {
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
          public Price(decimal cost,  decimal oldCost,Company company)
        {
            this.Cost = cost;
            this.Date = DateTime.Now;
            this.IsDiscont = true;
            this.OldCost = oldCost;
            this.Company = company;
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
        public decimal? Discont{get;set;}
        public DateTime Date { get; set; }
        public Company Company { get; set; }
        
    }
}
