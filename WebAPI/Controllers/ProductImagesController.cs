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
        [Route("{productId}")]
        public IList<ProductImageDTO> GetProductImages(int productId)
        {
            return _imageService.GetProductImages(productId);
        }

        [HttpPost]
        [Route("Product/{id}")]
        public IActionResult AddProductImages(int id, List<ProductImageDTO> productImages)
        {
            if (productImages.Count<1)
            {
                return BadRequest();
            }
            _imageService.AddProductImage(productImages, id);
            return Ok();
        }

        [HttpDelete]
        [Route("ProductImage/{id}")]
        public IActionResult DeleteProductImage(int id)
        {
            _imageService.DeleteProductImage(id);
            return NoContent();
        }
    }
}
