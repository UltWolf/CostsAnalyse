using System; 
using CostsAnalyse.Services.Data;

namespace CostsAnalyse.Models
{
    public class Changes
    {
        public int Id { get; set; }
        public bool isAdmin { get; set; }
        public int UserId{get;set;}
        public DateTime Date { get; set; }
        public  TypeOfChange change {get;set;}
        public int IdGoal { get; set; }         
    }
}