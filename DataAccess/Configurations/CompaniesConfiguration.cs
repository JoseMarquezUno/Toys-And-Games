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
    public class CompaniesConfiguration : IEntityTypeConfiguration<Company>
    {

        
        //TODO: If we already have a configuration file, try to use FluentAPI instead of DataAnnotations
        //this will let you have a more clean model.
        //TODO: Change name to CompaniesConfiguration
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50);

            builder.HasData(new Company { Id = 1, Name = "Mattel" },
                new Company { Id = 2, Name = "Hasbro"},
                new Company { Id = 3, Name = "Mi Alegria" });
        }
    }
}
