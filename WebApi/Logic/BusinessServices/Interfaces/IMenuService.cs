using Domain.Entities;

namespace WebApi.Logic.BusinessServices.Interfaces
{
    public interface IMenuService
    {
        Task<(IEnumerable<MenuPreset> Items, int TotalCount)> GetAllPresetsAsync(
            string? nameFilter = null,
            string? descriptionFilter = null,
            int pageIndex = 0,
            int pageSize = 50);
        Task<MenuPreset?> GetPresetByIdAsync(long id);
        Task<MenuPreset> AddPresetAsync(MenuPreset preset);
        Task<MenuPreset> UpdatePresetAsync(MenuPreset preset);
        Task DeletePresetAsync(long id);
        Task<(IEnumerable<MenuPresetItem> Items, int TotalCount)> GetAllPresetItemsAsync(
            long? menuPresetId = null,
            long? productId = null,
            int pageIndex = 0,
            int pageSize = 50);
        Task<MenuPresetItem?> GetPresetItemByIdAsync(long id);
        Task<MenuPresetItem> AddPresetItemAsync(MenuPresetItem item);
        Task<MenuPresetItem> UpdatePresetItemAsync(MenuPresetItem item);
        Task DeletePresetItemAsync(long id);
    }
}
