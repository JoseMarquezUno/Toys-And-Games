using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.Models.DTO;

namespace ToysAndGames.Services.Contracts
{
    public interface IProductImageService
    {
        void AddProductImage(IList<ProductImageDTO> productImages, int productId);
        void DeleteProductImage(string imageName);
        IList<ProductImageDTO> GetProductImages(int productId);
    }
}
