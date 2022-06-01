using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using ToysAndGames.Models.DTO;

namespace ToysAndGames.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly ToysAndGamesContext _context;
        public ProductService(ToysAndGamesContext context)
        {
            _context = context;
        }
        public int AddProduct(ProductDTO productDTO)
        {
            Product product = new()
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                AgeRestriction = productDTO.AgeRestriction,
                Price = productDTO.Price,
                CompanyId = productDTO.CompanyId
            };

            _context.Products.Add(product);
            _context.SaveChanges();
            return product.ProductId;
        }

        public void DeleteProduct(int id)
        {
            _context.Products.Remove(_context.Products.FirstOrDefault(p => p.ProductId == id));
            _context.SaveChanges();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Include(c => c.Company).FirstOrDefault(p => p.ProductId == id);
        }

        public IList<Product> GetProducts()
        {
            return _context.Products.Include(c=>c.Company).ToList();
        }

        public void UpdateProduct(int id, ProductDTO productDTO)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.AgeRestriction = productDTO.AgeRestriction;
            product.Price = productDTO.Price;
            product.CompanyId = productDTO.CompanyId;

            _context.Products.Update(product);

            _context.SaveChanges();
        }

        public bool ProductExists(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            return product != null;
        }
    }
}
