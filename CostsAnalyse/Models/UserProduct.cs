using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Models
{
    public class UserProduct
    {
        [Key]
        public string IdUserapp { get; set; }
        public UserApp Users { get; set; }
        [Key]
        public int IdProduct { get; set; }
        public Product Products { get; set; }
    }
}
