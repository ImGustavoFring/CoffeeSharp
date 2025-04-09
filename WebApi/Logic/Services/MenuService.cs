using Domain.Entities;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MenuPreset>> GetAllPresetsAsync()
        {
            return await _unitOfWork.MenuPresets.GetAllAsync();
        }

        public async Task<MenuPreset?> GetPresetByIdAsync(long id)
        {
            return await _unitOfWork.MenuPresets.GetByIdAsync(id);
        }

        public async Task<MenuPreset> AddPresetAsync(MenuPreset preset)
        {
            return await _unitOfWork.MenuPresets.AddAsync(preset);
        }

        public async Task<MenuPreset> UpdatePresetAsync(MenuPreset preset)
        {
            var existing = await _unitOfWork.MenuPresets.GetByIdAsync(preset.Id);
            if (existing == null)
            {
                throw new ArgumentException("MenuPreset not found.");
            }
            existing.Name = preset.Name;
            existing.Description = preset.Description;
            return await _unitOfWork.MenuPresets.UpdateAsync(existing);
        }

        public async Task DeletePresetAsync(long id)
        {
            await _unitOfWork.MenuPresets.DeleteAsync(id);
        }

        public async Task<IEnumerable<MenuPresetItem>> GetAllPresetItemsAsync()
        {
            return await _unitOfWork.MenuPresetItems.GetAllAsync();
        }

        public async Task<MenuPresetItem?> GetPresetItemByIdAsync(long id)
        {
            return await _unitOfWork.MenuPresetItems.GetByIdAsync(id);
        }

        public async Task<MenuPresetItem> AddPresetItemAsync(MenuPresetItem item)
        {
            var preset = await _unitOfWork.MenuPresets.GetByIdAsync(item.MenuPresetId);
            if (preset == null)
            {
                throw new ArgumentException("MenuPreset not found.");
            }
            return await _unitOfWork.MenuPresetItems.AddAsync(item);
        }

        public async Task<MenuPresetItem> UpdatePresetItemAsync(MenuPresetItem item)
        {
            var existing = await _unitOfWork.MenuPresetItems.GetByIdAsync(item.Id);
            if (existing == null)
            {
                throw new ArgumentException("MenuPresetItem not found.");
            }
            existing.ProductId = item.ProductId;
            existing.MenuPresetId = item.MenuPresetId;
            return await _unitOfWork.MenuPresetItems.UpdateAsync(existing);
        }

        public async Task DeletePresetItemAsync(long id)
        {
            await _unitOfWork.MenuPresetItems.DeleteAsync(id);
        }

        public async Task<IEnumerable<MenuPresetItem>> GetPresetItemsByPresetIdAsync(long presetId)
        {
            return await _unitOfWork.MenuPresetItems.GetAllAsync(
                filter: item => item.MenuPresetId == presetId);
        }
    }
}
