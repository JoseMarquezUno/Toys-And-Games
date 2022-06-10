using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;

namespace WebApiTests.Utilities
{
    public class CustomWebApplicationFactory<TProgram> :WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ToysAndGamesContext>));
                services.Remove(descriptor);

                services.AddDbContext<ToysAndGamesContext>(options =>
                {
                    options.UseInMemoryDatabase("ToysAndGamesInMemory");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ToysAndGamesContext>();

                    var productImages = scopedServices.GetRequiredService<IProductImageService>();

                    db.Database.EnsureCreated();

                    //try
                    //{
                    //    db.ProductImages.AddRange()
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw;
                    //}
                }
            });
        }
    }
}
