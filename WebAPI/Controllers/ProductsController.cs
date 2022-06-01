﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToysAndGames.Models;
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
        [HttpGet("Products")]
        public IList<Product> GetProducts()
        {
            return _productService.GetProducts();
        }

        [HttpGet("Product/{id}")]
        public Product GetProduct(int id)
        {
            return _productService.GetProductById(id);
        }

        [HttpPut("Product/{id}")]
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

        [HttpPost("Product")]
        public IActionResult AddProduct(ProductDTO productDTO)
        {
            int id = _productService.AddProduct(productDTO);
            return CreatedAtAction(nameof(GetProduct),
                new { id = id },
                productDTO); 
        }

        [HttpDelete("Product/{id}")]
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
