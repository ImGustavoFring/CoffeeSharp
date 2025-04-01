using Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class MenuPresetItemCrudService : IMenuPresetItemCrudService
    {
        private readonly IRepository<MenuPresetItem> _repository;

        public MenuPresetItemCrudService(IRepository<MenuPresetItem> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MenuPresetItem>> GetAllMenuPresetItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<MenuPresetItem?> GetMenuPresetItemByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<MenuPresetItem> AddMenuPresetItemAsync(MenuPresetItem item)
        {
            return await _repository.AddAsync(item);
        }

        public async Task<MenuPresetItem> UpdateMenuPresetItemAsync(MenuPresetItem item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task DeleteMenuPresetItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
