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
        Task<IEnumerable<MenuPresetItems>> GetAllPresetItemsAsync();
        Task<MenuPresetItems?> GetPresetItemByIdAsync(long id);
        Task<MenuPresetItems> AddPresetItemAsync(MenuPresetItems item);
        Task<MenuPresetItems> UpdatePresetItemAsync(MenuPresetItems item);
        Task DeletePresetItemAsync(long id);
        Task<IEnumerable<MenuPresetItems>> GetPresetItemsByPresetIdAsync(long presetId);
    }
}
