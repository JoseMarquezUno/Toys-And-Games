using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToysAndGames.Models;

namespace ToysAndGames.DataAccess.Configurations
{
    public class ProductsDataSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(new Product { ProductId = 1, Name = "Hot Wheels", Description = "Twin Mill", AgeRestriction = 3, CompanyId = 1, Price = 25 },
                new Product { ProductId = 2, Name = "Monopoly", AgeRestriction = 5, CompanyId = 2, Price = 220.5M },
                new Product { ProductId = 3, Name = "Maquina de Raspados", CompanyId = 3, Price = 620.35M },
                new Product { ProductId = 4, Name = "Hot Wheels", Description = "Twin Mill 2", AgeRestriction = 3, CompanyId = 1, Price = 25.88M });
        }
    }
}
