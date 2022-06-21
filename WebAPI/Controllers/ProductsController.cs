using Microsoft.AspNetCore.Mvc;
using ToysAndGames.Services.Contracts;
using WebAPI.DTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<IList<ProductDTO>>> GetProducts()
        {
            return Ok(await _productService.GetProducts());
        }

        [HttpGet]
        [Route("Product/{id}")]
        public async Task<ActionResult<ProductDTO?>> GetProduct(int id)
        {
            return Ok(await _productService.GetProductById(id));
        }

        [HttpPut]
        [Route("Product/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {
            try
            {
                if (await _productService.ProductExists(id))
                {
                    await _productService.UpdateProduct(id, productDTO);
                    return Ok(productDTO);
                }
                    return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("Product")]
        public async Task<ActionResult> AddProduct(ProductAddDTO productDTO)
        {
            try
            {
                await _productService.AddProduct(productDTO);
                //TODO: By naming convention these calls should be single lined, if its an object creation its ok to have it multiline
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Product/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                if (await _productService.ProductExists(id))
                {
                    await _productService.DeleteProduct(id);
                    return NoContent();
                }
                return NotFound($"Product with Id: {id} was not found in the database");

            }
            catch (Exception ex)
            {
                //TODO: Internal Server Error (500)
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            ////TODO: Line unreachable
            //return NoContent();
        }
    }
}
