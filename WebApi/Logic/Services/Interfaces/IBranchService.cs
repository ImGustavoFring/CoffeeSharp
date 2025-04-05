using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task<Branch?> GetBranchByIdAsync(long id);
        Task<Branch> AddBranchAsync(Branch branch);
        Task<Branch> UpdateBranchAsync(Branch branch);
        Task DeleteBranchAsync(long id);
        Task<IEnumerable<BranchMenu>> GetAllBranchMenusAsync();
        Task<IEnumerable<BranchMenu>> GetBranchMenusByBranchIdAsync(long branchId);
        Task<BranchMenu?> GetBranchMenuByIdAsync(long id);
        Task AssignMenuPresetToBranchAsync(long branchId, long menuPresetId);
        Task<BranchMenu> UpdateBranchMenuAvailabilityAsync(long branchMenuId, bool availability);
    }
}
