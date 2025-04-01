using Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IMenuPresetItemCrudService
    {
        Task<IEnumerable<MenuPresetItem>> GetAllMenuPresetItemsAsync();
        Task<MenuPresetItem?> GetMenuPresetItemByIdAsync(long id);
        Task<MenuPresetItem> AddMenuPresetItemAsync(MenuPresetItem item);
        Task<MenuPresetItem> UpdateMenuPresetItemAsync(MenuPresetItem item);
        Task DeleteMenuPresetItemAsync(long id);
    }
}
