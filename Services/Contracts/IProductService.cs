using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.Models;
using WebAPI.DTO;

namespace ToysAndGames.Services.Contracts
{
    public interface IProductService
    {
        Task<IList<ProductDTO>> GetProducts();
        Task<ProductDTO?> GetProductById(int id);
        Task AddProduct(ProductAddDTO productDTO);
        Task UpdateProduct(int id, ProductDTO productDTO);
        Task DeleteProduct(int id);
        Task<bool> ProductExists(int id);
    }
}
