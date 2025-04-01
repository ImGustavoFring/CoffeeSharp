using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
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

        public async Task<Category?> GetCategoryByIdAsync(int id)
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

        public async Task DeleteCategoryAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
