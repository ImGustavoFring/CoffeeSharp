using Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IMenuPresetItemService
    {
        Task<IEnumerable<MenuPresetItem>> GetAllMenuPresetItemsAsync();
        Task<MenuPresetItem?> GetMenuPresetItemByIdAsync(int id);
        Task<MenuPresetItem> AddMenuPresetItemAsync(MenuPresetItem item);
        Task<MenuPresetItem> UpdateMenuPresetItemAsync(MenuPresetItem item);
        Task DeleteMenuPresetItemAsync(int id);
    }
}
