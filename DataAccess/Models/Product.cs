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
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? AgeRestriction { get; set; }
        public decimal Price { get; set; }

        //TODO: Use IList<T> Instead of ICollection:
        //https://www.c-sharpcorner.com/UploadFile/78607b/difference-between-ienumerable-icollection-and-ilist-interf/
        //Navigation Properties
        public IList<ProductImage> ProductImages { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
