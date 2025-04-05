using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
{
    public class ProductCrudService : IProductCrudService
    {
        private readonly IRepository<Product> _repository;

        public ProductCrudService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            return await _repository.AddAsync(product);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _repository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
