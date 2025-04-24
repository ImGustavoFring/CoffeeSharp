using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using System.Linq.Expressions;
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
            return await _unitOfWork.MenuPresets.GetManyAsync();
        }

        public async Task<MenuPreset?> GetPresetByIdAsync(long id)
        {
            return await _unitOfWork.MenuPresets.GetByIdAsync(id);
        }

        public async Task<MenuPreset> AddPresetAsync(MenuPreset preset)
        {
            var result = await _unitOfWork.MenuPresets.AddOneAsync(preset);
            await _unitOfWork.SaveChangesAsync();

            return result;
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

            _unitOfWork.MenuPresets.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing;
        }

        public async Task DeletePresetAsync(long id)
        {
            await _unitOfWork.MenuPresets.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MenuPresetItem>> GetAllPresetItemsAsync()
        {
            return await _unitOfWork.MenuPresetItems.GetManyAsync();
        }

        public async Task<MenuPresetItem?> GetPresetItemByIdAsync(long id)
        {
            return await _unitOfWork.MenuPresetItems.GetByIdAsync(id);
        }

        public async Task<MenuPresetItem> AddPresetItemAsync(MenuPresetItem item)
        {
            var preset = await _unitOfWork.MenuPresets.GetByIdAsync(item.MenuPresetId) ?? throw new ArgumentException("MenuPreset not found.");
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId) ?? throw new ArgumentException("Product not found.");

            await _unitOfWork.MenuPresetItems.AddOneAsync(item);

            var branchMenus = await _unitOfWork.BranchMenus.GetManyAsync(
                filter: bm => bm.MenuPresetItems.MenuPresetId == item.MenuPresetId,
                includes: new List<Expression<Func<BranchMenu, object>>>
                {
                    bm => bm.MenuPresetItems
                }
            );

            var branchIds = branchMenus.Select(bm => bm.BranchId).Distinct();

            foreach (var branchId in branchIds)
            {
                var bm = new BranchMenu
                {
                    BranchId = branchId,
                    MenuPresetItems = item,
                    Availability = true
                };
                await _unitOfWork.BranchMenus.AddOneAsync(bm);
            }

            await _unitOfWork.SaveChangesAsync();

            return item;
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

            _unitOfWork.MenuPresetItems.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing;
        }

        public async Task DeletePresetItemAsync(long id)
        {
            var branchMenus = await _unitOfWork.BranchMenus.GetManyAsync(
                filter: bm => bm.MenuPresetItemsId == id
            );

            foreach (var bm in branchMenus)
            {
                await _unitOfWork.BranchMenus.DeleteAsync(bm.Id);
            }

            await _unitOfWork.MenuPresetItems.DeleteAsync(id);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MenuPresetItem>> GetPresetItemsByPresetIdAsync(long presetId)
        {
            return await _unitOfWork.MenuPresetItems.GetManyAsync(
                filter: item => item.MenuPresetId == presetId);
        }
    }
}
