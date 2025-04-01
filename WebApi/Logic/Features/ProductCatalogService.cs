using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Features.Interfaces;
using WebApi.Logic.Services;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Features
{
    public class ProductCatalogService : IProductCatalogService
    {
        private readonly IProductCrudService _productCrudService;
        private readonly ICategoryCrudService _categoryCrudService;

        public ProductCatalogService(IProductCrudService productCrudService, ICategoryCrudService categoryCrudService)
        {
            _productCrudService = productCrudService;
            _categoryCrudService = categoryCrudService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productCrudService.GetAllProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(long id)
        {
            return await _productCrudService.GetProductByIdAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var category = await _categoryCrudService.GetCategoryByIdAsync((int)product.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("Invalid category.");
            }
            return await _productCrudService.AddProductAsync(product);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var category = await _categoryCrudService.GetCategoryByIdAsync((int)product.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("Invalid category.");
            }
            return await _productCrudService.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(long id)
        {
            await _productCrudService.DeleteProductAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryCrudService.GetAllCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(long id)
        {
            return await _categoryCrudService.GetCategoryByIdAsync(id);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            return await _categoryCrudService.AddCategoryAsync(category);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            return await _categoryCrudService.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(long id)
        {
            await _categoryCrudService.DeleteCategoryAsync(id);
        }
    }
}
