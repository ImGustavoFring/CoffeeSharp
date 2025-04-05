using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;

namespace WebApi.Logic.Services
{
    public class ProductCatalogService : IProductCatalogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCatalogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(long id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("Invalid category.");
            }
            return await _unitOfWork.Products.AddAsync(product);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(product.Id);
            if (existingProduct == null) throw new ArgumentException("Product not found");

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;

            var category = await _unitOfWork.Categories.GetByIdAsync(existingProduct.CategoryId);
            if (category == null) throw new ArgumentException("Invalid category");

            return await _unitOfWork.Products.UpdateAsync(existingProduct);
        }

        public async Task DeleteProductAsync(long id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(long id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            return await _unitOfWork.Categories.AddAsync(category);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(category.Id);
            if (existingCategory == null)
            {
                throw new ArgumentException("Category not found.");
            }

            if (category.ParentId.HasValue)
            {
                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(category.ParentId.Value);
                if (parentCategory == null)
                {
                    throw new ArgumentException("Parent category not found.");
                }
            }

            existingCategory.Name = category.Name;
            existingCategory.ParentId = category.ParentId;

            return await _unitOfWork.Categories.UpdateAsync(existingCategory);
        }

        public async Task DeleteCategoryAsync(long id)
        {
            await _unitOfWork.Categories.DeleteAsync(id);
        }
    }
}
