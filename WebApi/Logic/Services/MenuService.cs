using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using System.Linq.Expressions;
using WebApi.Infrastructure.Extensions;
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

        public async Task<(IEnumerable<MenuPreset> Items, int TotalCount)> GetAllPresetsAsync(
            string? nameFilter = null,
            string? descriptionFilter = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<MenuPreset, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(nameFilter))
                filter = filter.AndAlso(p => p.Name.Contains(nameFilter));

            if (!string.IsNullOrWhiteSpace(descriptionFilter))
                filter = filter.AndAlso(p => p.Description!.Contains(descriptionFilter));

            var total = await _unitOfWork.MenuPresets.CountAsync(filter);

            var items = await _unitOfWork.MenuPresets.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(p => p.Name),
                includes: null,
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (items, total);
        }

        public async Task<MenuPreset?> GetPresetByIdAsync(long id)
        {
            return await _unitOfWork.MenuPresets.GetByIdAsync(id);
        }

        public async Task<MenuPreset> AddPresetAsync(MenuPreset preset) // add menu preset items
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

        public async Task<(IEnumerable<MenuPresetItem> Items, int TotalCount)> GetAllPresetItemsAsync(
            long? menuPresetId = null,
            long? productId = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<MenuPresetItem, bool>> filter = i => true;

            if (menuPresetId.HasValue)
                filter = filter.AndAlso(i => i.MenuPresetId == menuPresetId.Value);

            if (productId.HasValue)
                filter = filter.AndAlso(i => i.ProductId == productId.Value);

            var total = await _unitOfWork.MenuPresetItems.CountAsync(filter);

            var items = await _unitOfWork.MenuPresetItems.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(i => i.Id),
                includes: new List<Expression<Func<MenuPresetItem, object>>>
                {
                    i => i.Product,
                    i => i.MenuPreset
                },
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (items, total);
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
    }
}
