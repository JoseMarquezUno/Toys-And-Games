using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.Models;
using ToysAndGames.Models.DTO;

namespace ToysAndGames.Services.Contracts
{
    public interface IProductService
    {
        IList<Product> GetProducts();
        Product GetProductById(int id);
        int AddProduct(ProductDTO productDTO);
        void UpdateProduct(int id, ProductDTO productDTO);
        void DeleteProduct(int id);
        bool ProductExists(int id);
    }
}
