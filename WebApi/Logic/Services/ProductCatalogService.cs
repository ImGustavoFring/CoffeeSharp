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
            return await _unitOfWork.Products.GetManyAsync();
        }

        public async Task<Product?> GetProductByIdAsync(long id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        public async Task<Product> AddProductAsync(long categoryId, Product product)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);

            if (category == null)
            {
                throw new ArgumentException("Invalid category.");
            }

            product.CategoryId = category.Id;

            var result = await _unitOfWork.Products.AddOneAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(product.Id);

            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found");
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);

            if (category == null)
            {
                throw new ArgumentException("Invalid category");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = category.Id;

            _unitOfWork.Products.Update(existingProduct);
            await _unitOfWork.SaveChangesAsync();

            return existingProduct;
        }

        public async Task DeleteProductAsync(long id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetManyAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(long id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var result = await _unitOfWork.Categories.AddOneAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return result;
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

            var existingParentCategory = await _unitOfWork.Categories.GetByIdAsync(category.ParentId);

            existingCategory.Name = category.Name;
            existingCategory.ParentId = existingParentCategory.Id;

            _unitOfWork.Categories.Update(existingCategory);
            await _unitOfWork.SaveChangesAsync();

            return existingCategory;
        }

        public async Task DeleteCategoryAsync(long id)
        {
            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
