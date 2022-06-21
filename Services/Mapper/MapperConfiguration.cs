using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.Models;
using WebAPI.DTO;

namespace WebAPI.Services.Mapper
{
    public class MapperConfiguration :Profile
    {
        public MapperConfiguration()
        {
            //Product
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductAddDTO>().ReverseMap();

            //Company
            CreateMap<Company, CompanyDTO>().ReverseMap();

            //ProductImages
            CreateMap<ProductImage,ProductImageDTO>().ReverseMap();
        }
    }
}
