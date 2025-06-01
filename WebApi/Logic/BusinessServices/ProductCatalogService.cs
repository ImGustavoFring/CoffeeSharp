using CoffeeSharp.Domain.Entities;
using WebApi.Logic.BusinessServices.Interfaces;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using System.Linq.Expressions;
using WebApi.Infrastructure.Extensions;

namespace WebApi.Logic.BusinessServices
{
    public class ProductCatalogService : IProductCatalogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCatalogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetAllProductsAsync(
            string? nameFilter = null,
            long? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<Product, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(nameFilter))
                filter = filter.AndAlso(p => p.Name.Contains(nameFilter));

            if (categoryId.HasValue)
                filter = filter.AndAlso(p => p.CategoryId == categoryId.Value);

            if (minPrice.HasValue)
                filter = filter.AndAlso(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                filter = filter.AndAlso(p => p.Price <= maxPrice.Value);

            var total = await _unitOfWork.Products.CountAsync(filter);

            var items = await _unitOfWork.Products.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(p => p.Name),
                includes: new List<Expression<Func<Product, object>>> { p => p.Category },
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (items, total);
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

        public async Task<(IEnumerable<Category> Items, int TotalCount)> GetAllCategoriesAsync(
            string? nameFilter = null,
            long? parentCategoryId = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<Category, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(nameFilter))
                filter = filter.AndAlso(c => c.Name.Contains(nameFilter));

            if (parentCategoryId.HasValue)
                filter = filter.AndAlso(c => c.ParentCategoryId == parentCategoryId.Value);

            var total = await _unitOfWork.Categories.CountAsync(filter);

            var items = await _unitOfWork.Categories.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(c => c.Name),
                includes: new List<Expression<Func<Category, object>>> { c => c.ParentCategory },
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (items, total);
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

            if (category.ParentCategoryId.HasValue)
            {
                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(category.ParentCategoryId.Value);

                if (parentCategory == null)
                {
                    throw new ArgumentException("Parent category not found.");
                }
            }

            var existingParentCategory = await _unitOfWork.Categories.GetByIdAsync(category.ParentCategoryId);

            existingCategory.Name = category.Name;
            existingCategory.ParentCategoryId = existingParentCategory.Id;

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
