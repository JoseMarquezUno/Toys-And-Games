using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToysAndGames.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
