using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Utilities;
using WebAPI.DTO;
using AutoMapper;

namespace ToysAndGames.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly ToysAndGamesContext _context;
        private readonly IMapper _mapper;
        public ProductService(ToysAndGamesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddProduct(ProductAddDTO productDTO)
        {
            //TODO: Change signature to void and throw exception in case the operation failed
            try
            {
                if (productDTO is not null)
                {
                    Product product =
                    // new()
                    //{
                    //    Name = productDTO.Name,
                    //    Description = productDTO.Description,
                    //    AgeRestriction = productDTO.AgeRestriction,
                    //    Price = productDTO.Price,
                    //    CompanyId = productDTO.CompanyId
                    //};
                    _mapper.Map<Product>(productDTO);

                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product is not null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductDTO?> GetProductById(int id)
        {
            try
            {
                string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var product = await _context.Products.Include(c => c.Company)
                    .Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

                if (product is not null && path is not null)
                {
                    List<string> relPaths = ImageUtilities.GetImagePathsFromAssembly(path, product.ProductImages.Select(p => p.ImagePath).ToList());
                    ProductDTO productDTO = _mapper.Map<ProductDTO>(product);
                    //    new()
                    //{
                    //    Id = product.Id,
                    //    Name = product.Name,
                    //    AgeRestriction = product.AgeRestriction,
                    //    Description = product.Description,
                    //    Price = product.Price,
                    //    CompanyId = product.CompanyId,
                    //    CompanyName = product.Company.Name
                    //};
                    return productDTO;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IList<ProductDTO>> GetProducts()
        {
            List<ProductDTO> productsDto = new();
            try
            {
                var products = await _context.Products.Include(c => c.Company).ToListAsync();
                products.ForEach(p => productsDto.Add( _mapper.Map<ProductDTO>(p)
                //    new()
                //{
                //    Id = p.Id,
                //    Name = p.Name,
                //    AgeRestriction = p.AgeRestriction,
                //    Price = p.Price,
                //    CompanyName = p.Company.Name
                //}
                    ));
                return productsDto;
            }
            catch (Exception)
            {
                return productsDto;
            }
        }

        public async Task UpdateProduct(int id, ProductDTO productDTO)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product is not null)
                {
                    product.Name = productDTO.Name;
                    product.Description = productDTO.Description;
                    product.AgeRestriction = productDTO.AgeRestriction;
                    product.Price = productDTO.Price;
                    product.CompanyId = productDTO.CompanyId;

                    _context.Products.Update(product);

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ProductExists(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

                return product is not null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
