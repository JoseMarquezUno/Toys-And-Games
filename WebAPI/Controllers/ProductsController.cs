using Microsoft.AspNetCore.Mvc;
using ToysAndGames.Models.DTO;
using ToysAndGames.Services.Contracts;

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
        public IList<ProductDTO> GetProducts()
        {
            return _productService.GetProducts();
        }

        [HttpGet]
        [Route("Product/{id}")]
        public ProductDTO GetProduct(int id)
        {
            return _productService.GetProductById(id);
        }

        [HttpPut]
        [Route("Product/{id}")]
        public IActionResult UpdateProduct(int id, ProductDTO productDTO)
        {
            if (_productService.ProductExists(id))
            {
                _productService.UpdateProduct(id, productDTO); 
            }
            else
            {
                return NotFound();
            }
            return Ok(productDTO);
        }

        [HttpPost]
        [Route("Product")]
        public IActionResult AddProduct(ProductDTO productDTO)
        {
            int id = _productService.AddProduct(productDTO);
            return CreatedAtAction(nameof(GetProduct),
                new { id = id },
                productDTO); 
        }

        [HttpDelete]
        [Route("Product/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (_productService.ProductExists(id))
            {
                _productService.DeleteProduct(id);
            }
            else
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
