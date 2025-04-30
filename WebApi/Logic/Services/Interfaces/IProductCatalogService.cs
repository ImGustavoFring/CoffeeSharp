using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IProductCatalogService
    {
        Task<Category> AddCategoryAsync(Category category);
        Task<Product> AddProductAsync(long categoryId, Product product);
        Task DeleteCategoryAsync(long id);
        Task DeleteProductAsync(long id);
        Task<(IEnumerable<Category> Items, int TotalCount)> GetAllCategoriesAsync(
            string? nameFilter = null,
            long? parentCategoryId = null,
            int pageIndex = 0,
            int pageSize = 50);
        Task<(IEnumerable<Product> Items, int TotalCount)> GetAllProductsAsync(
            string? nameFilter = null,
            long? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int pageIndex = 0,
            int pageSize = 50);
        Task<Category?> GetCategoryByIdAsync(long id);
        Task<Product?> GetProductByIdAsync(long id);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<Product> UpdateProductAsync(Product product);
    }
}