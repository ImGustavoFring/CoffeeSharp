using Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class MenuPresetCrudService : IMenuPresetCrudService
    {
        private readonly IRepository<MenuPreset> _repository;

        public MenuPresetCrudService(IRepository<MenuPreset> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MenuPreset>> GetAllMenuPresetsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<MenuPreset?> GetMenuPresetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<MenuPreset> AddMenuPresetAsync(MenuPreset preset)
        {
            return await _repository.AddAsync(preset);
        }

        public async Task<MenuPreset> UpdateMenuPresetAsync(MenuPreset preset)
        {
            return await _repository.UpdateAsync(preset);
        }

        public async Task DeleteMenuPresetAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
