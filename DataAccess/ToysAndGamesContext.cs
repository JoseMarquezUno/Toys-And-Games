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
    //TODO: In terms of workspaces i would have Models inside the DataAccess and the DTO in his own project
    //this is because the models will always be matched/attached to the data context anyways.

    //As far as naming convensions of the project the naming its usually MyAPIProject.MyLibrary ex: WebAPI.DataAccess
    //This is because you can have multiple APIS with multiple models and multiple dataAccess layers like MyOtherWebAPI.DataAccess
    public class ToysAndGamesContext : DbContext
    {
        
        public ToysAndGamesContext(DbContextOptions<ToysAndGamesContext> options) : base(options)
        {

        }
        /// <summary>
        /// WARNING: DO NOT DELETE!!!! Empty constructor is used for Mocking DbContext in the Unit Test
        /// </summary>
        public ToysAndGamesContext()
        {

        }
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("Money");
            modelBuilder.ApplyConfiguration(new CompaniesDataSeed());
            modelBuilder.ApplyConfiguration(new ProductsDataSeed());
            modelBuilder.ApplyConfiguration(new ProductImagesDataSeed());
        }
    }
}
