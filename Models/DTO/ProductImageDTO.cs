using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToysAndGames.Models.DTO
{
    public class ProductImageDTO
    {
        public int? ProductImageId { get; set; }
        public string ImageBase64 { get; set; }
        public string? Name { get; set; }
    }
}
