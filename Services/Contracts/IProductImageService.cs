using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DTO;

namespace ToysAndGames.Services.Contracts
{
    public interface IProductImageService
    {
        Task AddProductImage(IList<ProductImageDTO> productImages, int productId);
        Task DeleteProductImage(int productImageId);
        Task<IList<ProductImageDTO>> GetProductImages(int productId);
        Task<bool> ProductImageExists(int id);
    }
}
