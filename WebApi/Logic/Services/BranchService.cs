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

        public async Task<IEnumerable<Branch>> GetBranchesAsync(string? searchTerm, int pageIndex, int pageSize)
        {
            Expression<Func<Branch, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string lowered = searchTerm.ToLower();

                filter = b =>
                    b.Name.ToLower().Contains(lowered) ||
                    b.Address.ToLower().Contains(lowered); // think about optimization
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
            var existing = await _unitOfWork.Branches.GetByIdAsync(branch.Id);

            if (existing == null)
            {
                throw new ArgumentException("Branch not found.");
            }

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
            long? menuPresetItemsId,
            bool? availability,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<BranchMenu, bool>>? filter = null;

            if (branchId.HasValue)
                filter = filter == null
                    ? bm => bm.BranchId == branchId.Value
                    : filter.AndAlso(bm => bm.BranchId == branchId.Value);

            if (menuPresetItemsId.HasValue)
                filter = filter == null
                    ? bm => bm.MenuPresetItemsId == menuPresetItemsId.Value
                    : filter.AndAlso(bm => bm.MenuPresetItemsId == menuPresetItemsId.Value);

            if (availability.HasValue)
                filter = filter == null
                    ? bm => bm.Availability == availability.Value
                    : filter.AndAlso(bm => bm.Availability == availability.Value);

            return await _unitOfWork.BranchMenus.GetManyAsync(
                filter: filter,
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
            var branch = await _unitOfWork.Branches.GetByIdAsync(branchId);

            if (branch == null)
            {
                throw new ArgumentException("Branch not found.");
            }

            var preset = await _unitOfWork.MenuPresets.GetByIdAsync(menuPresetId);

            if (preset == null)
            {
                throw new ArgumentException("MenuPreset not found.");
            }

            var existingMenus = await _unitOfWork.BranchMenus.GetManyAsync(
                filter: bm => bm.BranchId == branchId);

            foreach (var bm in existingMenus)
            {
                await _unitOfWork.BranchMenus.DeleteAsync(bm.Id);
            }

            var presetItems = await _unitOfWork.MenuPresetItems.GetManyAsync(
                filter: mpi => mpi.MenuPresetId == menuPresetId);

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
            var existing = await _unitOfWork.BranchMenus.GetByIdAsync(branchMenuId);

            if (existing == null)
            {
                throw new ArgumentException("BranchMenu not found.");
            }

            existing.Availability = availability;

            _unitOfWork.BranchMenus.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing;
        }
    }
}
