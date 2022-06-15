using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToysAndGames.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [Range(0,100)]
        public int? AgeRestriction { get; set; }
        public int CompanyId { get; set; }
        
        [Range(1.00,1000.00)]
        public decimal Price { get; set; }

        //TODO: Use IList<T> Instead of ICollection:
        //https://www.c-sharpcorner.com/UploadFile/78607b/difference-between-ienumerable-icollection-and-ilist-interf/
        //Navigation Properties
        public ICollection<ProductImage> ProductImages { get; set; }
        public Company Company { get; set; }
    }
}
