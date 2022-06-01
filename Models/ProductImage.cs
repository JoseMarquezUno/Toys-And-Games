using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToysAndGames.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        [Required]
        public string ImagePath { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
