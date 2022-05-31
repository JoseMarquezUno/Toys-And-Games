using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.Models;

namespace ToysAndGames.DataAccess.Configurations
{
    public class CompaniesDataSeed : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(new { CompanyId = 1, Name = "Mattel" },
                new { CompanyId = 2, Name = "Hasbro"},
                new { CompanyId = 3, Name = "Mi Alegria" });
        }
    }
}
