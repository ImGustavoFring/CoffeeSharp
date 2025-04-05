using Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuPreset>> GetAllPresetsAsync();
        Task<MenuPreset?> GetPresetByIdAsync(long id);
        Task<MenuPreset> AddPresetAsync(MenuPreset preset);
        Task<MenuPreset> UpdatePresetAsync(MenuPreset preset);
        Task DeletePresetAsync(long id);
        Task<IEnumerable<MenuPresetItem>> GetAllPresetItemsAsync();
        Task<MenuPresetItem?> GetPresetItemByIdAsync(long id);
        Task<MenuPresetItem> AddPresetItemAsync(MenuPresetItem item);
        Task<MenuPresetItem> UpdatePresetItemAsync(MenuPresetItem item);
        Task DeletePresetItemAsync(long id);
        Task<IEnumerable<MenuPresetItem>> GetPresetItemsByPresetIdAsync(long presetId);
    }
}
