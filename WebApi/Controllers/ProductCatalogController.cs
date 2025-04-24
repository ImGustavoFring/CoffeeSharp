using CoffeeSharp.Domain.Entities;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCatalogController : ControllerBase
    {
        private readonly IProductCatalogService _productCatalogService;

        public ProductCatalogController(IProductCatalogService productCatalogService)
        {
            _productCatalogService = productCatalogService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            IEnumerable<Product> products = await _productCatalogService.GetAllProductsAsync();
            IEnumerable<ProductDto> productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId
            });
            return Ok(productDtos);
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            Product? product = await _productCatalogService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
            return Ok(productDto);
        }

        [HttpPost("products")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };

            Product createdProduct = await _productCatalogService.AddProductAsync(request.CategoryId, product);

            var productDto = new ProductDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                CategoryId = createdProduct.CategoryId
            };

            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
        }

        [HttpPut("products/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] UpdateProductRequest request)
        {
            if (id != request.Id)
            {
                ModelState.AddModelError("Id", "URL id does not match request body id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId
            };

            Product updatedProduct = await _productCatalogService.UpdateProductAsync(product);

            var productDto = new ProductDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                CategoryId = updatedProduct.CategoryId
            };

            return Ok(productDto);
        }

        [HttpDelete("products/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            await _productCatalogService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            IEnumerable<Category> categories = await _productCatalogService.GetAllCategoriesAsync();
            IEnumerable<CategoryDto> categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentCategoryId
            });
            return Ok(categoryDtos);
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryById(long id)
        {
            Category? category = await _productCatalogService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentCategoryId
            };
            return Ok(categoryDto);
        }

        [HttpPost("categories")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Name = request.Name,
                ParentCategoryId = request.ParentId
            };

            Category createdCategory = await _productCatalogService.AddCategoryAsync(category);
            var categoryDto = new CategoryDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                ParentId = createdCategory.ParentCategoryId
            };

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id }, categoryDto);
        }

        [HttpPut("categories/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateCategory(long id, [FromBody] UpdateCategoryRequest request)
        {
            if (id != request.Id)
            {
                ModelState.AddModelError("Id", "URL id does not match request body id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Id = request.Id,
                Name = request.Name,
                ParentCategoryId = request.ParentId
            };

            Category updatedCategory = await _productCatalogService.UpdateCategoryAsync(category);
            var categoryDto = new CategoryDto
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                ParentId = updatedCategory.ParentCategoryId
            };

            return Ok(categoryDto);
        }

        [HttpDelete("categories/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            await _productCatalogService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
