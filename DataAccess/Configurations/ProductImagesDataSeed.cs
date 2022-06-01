using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.Models;

namespace ToysAndGames.DataAccess.Configurations
{
    public class ProductImagesDataSeed : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {            
            builder.HasData(new ProductImage { ProductImageId = 1, ProductId = 1, ImagePath = "Images\\TwinMill.jpg" });            
            builder.HasData(new ProductImage { ProductImageId = 2, ProductId = 2, ImagePath = "Images\\Monopoly.jfif" });
            builder.HasData(new ProductImage { ProductImageId = 3, ProductId = 3, ImagePath = "Images\\maquinaderaspados.jpg" });
            builder.HasData(new ProductImage { ProductImageId = 4, ProductId = 4, ImagePath = "Images\\TwinMill2.jpg" });
        }
    }
}
