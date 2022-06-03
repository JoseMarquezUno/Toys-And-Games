using Microsoft.AspNetCore.Mvc;
using ToysAndGames.Models.DTO;
using ToysAndGames.Services.Contracts;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _imageService;
        public ProductImagesController(IProductImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public IList<ProductImageDTO> GetProductImages(int productId)
        {
            return _imageService.GetProductImages(productId);
        }

        [HttpPost]
        [Route("Product/{id}")]
        public IActionResult AddProductImages(List<ProductImageDTO> productImages, int id)
        {
            if (productImages.Count<1)
            {
                return BadRequest();
            }
            _imageService.AddProductImage(productImages, id);
            return CreatedAtAction(nameof(GetProductImages),id,value:null);
        }

        [HttpDelete]
        [Route("ProductImage/{imageName}")]
        public IActionResult DeleteProductImage(string imageName)
        {
            _imageService.DeleteProductImage(imageName);
            return NoContent();
        }
    }
}
