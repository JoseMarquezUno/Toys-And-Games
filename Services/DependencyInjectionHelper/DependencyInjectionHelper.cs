using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;
using ToysAndGames.Services.Contracts;
using ToysAndGames.Services.Services;
using WebAPI.Services.Mapper;

namespace WebAPI.Services.DIHelper
{
    public static class DependencyInjectionHelper
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MapperConfiguration).Assembly);
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IProductImageService, ProductImageService>();

            return services;
        }
    }
}
