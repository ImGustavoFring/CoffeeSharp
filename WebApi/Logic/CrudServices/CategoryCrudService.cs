using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
{
    public class CategoryCrudService : ICategoryCrudService
    {
        private readonly IRepository<Category> _repository;

        public CategoryCrudService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            return await _repository.AddAsync(category);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            return await _repository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
