using Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IMenuPresetService
    {
        Task<IEnumerable<MenuPreset>> GetAllMenuPresetsAsync();
        Task<MenuPreset?> GetMenuPresetByIdAsync(int id);
        Task<MenuPreset> AddMenuPresetAsync(MenuPreset preset);
        Task<MenuPreset> UpdateMenuPresetAsync(MenuPreset preset);
        Task DeleteMenuPresetAsync(int id);
    }
}
