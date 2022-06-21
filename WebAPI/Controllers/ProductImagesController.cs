using Microsoft.AspNetCore.Mvc;
using ToysAndGames.Services.Contracts;
using WebAPI.DTO;

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
        public async Task<ActionResult<IList<ProductImageDTO>>> GetProductImages(int productId)
        {
            return Ok(await _imageService.GetProductImages(productId));
        }

        [HttpPost]
        [Route("Product/{id}")]
        public async Task<ActionResult> AddProductImages(int id, List<ProductImageDTO> productImages)
        {
            try
            {
                if (productImages.Count < 1)
                {
                    return BadRequest();
                }
                await _imageService.AddProductImage(productImages, id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("ProductImage/{id}")]
        public async Task<ActionResult> DeleteProductImage(int id)
        {
            try
            {
                if (await _imageService.ProductImageExists(id))
                {
                    await _imageService.DeleteProductImage(id);
                    return NoContent(); 
                }
                return NotFound($"ProducImage with Id: {id} was not found in database");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
