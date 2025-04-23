using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IProductCatalogService
    {
        Task<Category> AddCategoryAsync(Category category);
        Task<Product> AddProductAsync(long categoryId, Product product);
        Task DeleteCategoryAsync(long id);
        Task DeleteProductAsync(long id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Category?> GetCategoryByIdAsync(long id);
        Task<Product?> GetProductByIdAsync(long id);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<Product> UpdateProductAsync(Product product);
    }
}