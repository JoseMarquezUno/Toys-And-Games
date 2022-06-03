using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToysAndGames.Models.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [Range(0, 100)]
        public int? AgeRestriction { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        [Range(1.00, 1000.00)]
        public decimal Price { get; set; }
        public IList<ProductImageDTO> ProductImage { get; set; }
    }
}
