using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Contracts.Requests;
using Product.Api.Mapping;
using Product.Api.Services;

namespace Product.Api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var product = request.MapToProduct();
            var result = await _productService.CreateAsync(product);
            return CreatedAtAction("GetById", new { productId = result.Id }, result.MapToProductResponse());
        }

        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid productId)
        {
            var product = await _productService.GetAsync(productId);
            return product is null ? NotFound() : Ok(product.MapToProductResponse());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            var productResponses = products.Select(x => x.MapToProductResponse());
            return Ok(productResponses);
        }

        [HttpDelete("{productId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid productId)
        {
            await _productService.DeleteAsync(productId);
            return NoContent();
        }
    }
}
