using CoffeeSharp.Domain.Entities;
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

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _unitOfWork.Branches.GetAllAsync();
        }

        public async Task<Branch?> GetBranchByIdAsync(long id)
        {
            return await _unitOfWork.Branches.GetByIdAsync(id);
        }

        public async Task<Branch> AddBranchAsync(Branch branch)
        {
            return await _unitOfWork.Branches.AddAsync(branch);
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
            return await _unitOfWork.Branches.UpdateAsync(existing);
        }

        public async Task DeleteBranchAsync(long id)
        {
            await _unitOfWork.Branches.DeleteAsync(id);
        }

        public async Task<IEnumerable<BranchMenu>> GetAllBranchMenusAsync()
        {
            return await _unitOfWork.BranchMenus.GetAllAsync();
        }

        public async Task<IEnumerable<BranchMenu>> GetBranchMenusByBranchIdAsync(long branchId)
        {
            return await _unitOfWork.BranchMenus.GetAllAsync(filter: bm => bm.BranchId == branchId);
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
            var existingMenus = await _unitOfWork.BranchMenus.GetAllAsync(filter: bm => bm.BranchId == branchId);
            foreach (var bm in existingMenus)
            {
                await _unitOfWork.BranchMenus.DeleteAsync(bm.Id);
            }
            var presetItems = await _unitOfWork.MenuPresetItems.GetAllAsync(filter: mpi => mpi.MenuPresetId == menuPresetId);
            foreach (var item in presetItems)
            {
                var branchMenu = new BranchMenu
                {
                    BranchId = branchId,
                    MenuPresetItemId = item.Id,
                    Availability = true
                };
                await _unitOfWork.BranchMenus.AddAsync(branchMenu);
            }
        }

        public async Task<BranchMenu> UpdateBranchMenuAvailabilityAsync(long branchMenuId, bool availability)
        {
            var existing = await _unitOfWork.BranchMenus.GetByIdAsync(branchMenuId);
            if (existing == null)
            {
                throw new ArgumentException("BranchMenu not found.");
            }
            existing.Availability = availability;
            return await _unitOfWork.BranchMenus.UpdateAsync(existing);
        }
    }
}
