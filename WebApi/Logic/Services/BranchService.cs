using System.Linq.Expressions;
using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Extensions;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Branch>> GetBranchesAsync(
            string? name,
            string? address,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<Branch, bool>>? filter = null;

            // the method is case sensitive to input data - a hard match of input substrings is required
            if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(address))
            {
                filter = branch =>
                    (string.IsNullOrWhiteSpace(name) || branch.Name.Contains(name)) &&
                    (string.IsNullOrWhiteSpace(address) || branch.Address.Contains(address));
            }

            return await _unitOfWork.Branches.GetManyAsync(
                filter: filter,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }


        public async Task<Branch?> GetBranchByIdAsync(long id)
        {
            return await _unitOfWork.Branches.GetByIdAsync(id);
        }

        public async Task<Branch> AddBranchAsync(Branch branch)
        {
            var result = await _unitOfWork.Branches.AddOneAsync(branch);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Branch> UpdateBranchAsync(Branch branch)
        {
            var existing = await _unitOfWork.Branches.GetByIdAsync(branch.Id) 
                ?? throw new ArgumentException("Branch not found.");

            existing.Name = branch.Name;
            existing.Address = branch.Address;

            _unitOfWork.Branches.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteBranchAsync(long id)
        {
            await _unitOfWork.Branches.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<BranchMenu>> GetAllBranchMenusAsync(
            long? branchId,
            long? menuPresetItemId,
            long? menuPresetId,
            bool? availability,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<BranchMenu, bool>>? filter = null;

            if (branchId.HasValue)
                filter = filter == null
                    ? branchMenu => branchMenu.BranchId == branchId.Value
                    : filter.AndAlso(branchMenu => branchMenu.BranchId == branchId.Value);

            if (menuPresetItemId.HasValue)
                filter = filter == null
                    ? branchMenu => branchMenu.MenuPresetItemsId == menuPresetItemId.Value
                    : filter.AndAlso(branchMenu => branchMenu.MenuPresetItemsId == menuPresetItemId.Value);

            if (menuPresetId.HasValue)
                filter = filter == null
                    ? branchMenu => branchMenu.MenuPresetItems != null &&
                                    branchMenu.MenuPresetItems.MenuPresetId == menuPresetId.Value
                    : filter.AndAlso(branchMenu => branchMenu.MenuPresetItems != null &&
                                                   branchMenu.MenuPresetItems.MenuPresetId == menuPresetId.Value);

            if (availability.HasValue)
                filter = filter == null
                    ? branchMenu => branchMenu.Availability == availability.Value
                    : filter.AndAlso(branchMenu => branchMenu.Availability == availability.Value);

            return await _unitOfWork.BranchMenus.GetManyAsync(
                filter: filter,
                includes: new List<Expression<Func<BranchMenu, object>>>
                {
                    branchMenu => branchMenu.MenuPresetItems
                },
                disableTracking: false,
                pageIndex: pageIndex,
                pageSize: pageSize);
        }

        public async Task<BranchMenu?> GetBranchMenuByIdAsync(long id)
        {
            return await _unitOfWork.BranchMenus.GetByIdAsync(id);
        }

        public async Task AssignMenuPresetToBranchAsync(long branchId, long menuPresetId)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(branchId) 
                ?? throw new ArgumentException("Branch not found.");


            var preset = await _unitOfWork.MenuPresets.GetByIdAsync(menuPresetId) 
                ?? throw new ArgumentException("MenuPreset not found."); ;

            var existingMenus = await _unitOfWork.BranchMenus.GetManyAsync(
                filter: branchMenus => branchMenus.BranchId == branchId);

            foreach (var branchMenu in existingMenus) 
            {
                await _unitOfWork.BranchMenus.DeleteAsync(branchMenu.Id);
            }

            var presetItems = await _unitOfWork.MenuPresetItems.GetManyAsync(
                filter: menuPresetItems => menuPresetItems.MenuPresetId == menuPresetId);

            foreach (var item in presetItems)
            {
                var branchMenu = new BranchMenu
                {
                    BranchId = branch.Id,
                    MenuPresetItemsId = item.Id,
                    Availability = true
                };

                await _unitOfWork.BranchMenus.AddOneAsync(branchMenu);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<BranchMenu> UpdateBranchMenuAvailabilityAsync(long branchMenuId, bool availability)
        {
            var existing = await _unitOfWork.BranchMenus.GetByIdAsync(branchMenuId) 
                ?? throw new ArgumentException("BranchMenu not found."); ;

            existing.Availability = availability;

            _unitOfWork.BranchMenus.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing;
        }
    }
}
