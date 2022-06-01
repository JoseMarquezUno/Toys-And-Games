using Microsoft.EntityFrameworkCore;
using ToysAndGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess.Configurations;

namespace ToysAndGames.DataAccess
{
    public class ToysAndGamesContext : DbContext
    {
        public ToysAndGamesContext(DbContextOptions<ToysAndGamesContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("Money");
            modelBuilder.ApplyConfiguration(new CompaniesDataSeed());
            modelBuilder.ApplyConfiguration(new ProductsDataSeed());
            modelBuilder.ApplyConfiguration(new ProductImagesDataSeed());
        }
    }
}
