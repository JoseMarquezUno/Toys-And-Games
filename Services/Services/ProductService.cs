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
using System.Reflection;
using Utilities;

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
            //TODO: Change signature to void and throw exception in case the operation failed
            if (productDTO != null)
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
            //TODO: Explain this :")
            return -1;
        }

        public void DeleteProduct(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public ProductDTO GetProductById(int id)
        {
            string path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            Product product = _context.Products.Include(c => c.Company).Include(p => p.ProductImages).FirstOrDefault(p => p.ProductId == id);
            if (product!=null)
            {
                List<string> relPaths = ImageUtilities.GetImagePathsFromAssembly(path, product.ProductImages.Select(p => p.ImagePath).ToList());
                ProductDTO productDTO = new()
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    AgeRestriction = product.AgeRestriction,
                    Description = product.Description,
                    Price = product.Price,
                    CompanyId = product.CompanyId,
                    CompanyName = product.Company.Name
                };
                return productDTO; 
            }
            return null;
        }

        public IList<ProductDTO> GetProducts()
        {
            List<ProductDTO> products = new();
            _context.Products.Include(c => c.Company).ToList().ForEach(p => products.Add(new()
            {
                ProductId = p.ProductId,
                Name = p.Name,
                AgeRestriction = p.AgeRestriction,
                Price = p.Price,
                CompanyName = p.Company.Name
            }));
            return products;
        }

        public void UpdateProduct(int id, ProductDTO productDTO)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product != null)
            {
                product.Name = productDTO.Name;
                product.Description = productDTO.Description;
                product.AgeRestriction = productDTO.AgeRestriction;
                product.Price = productDTO.Price;
                product.CompanyId = productDTO.CompanyId;

                _context.Products.Update(product);

                _context.SaveChanges();
            }
        }

        public bool ProductExists(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            return product != null;
        }
    }
}
