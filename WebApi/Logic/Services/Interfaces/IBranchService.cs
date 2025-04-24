using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<Branch>> GetBranchesAsync(string? name,
            string? address, int pageIndex,
            int pageSize);
        Task<Branch?> GetBranchByIdAsync(long id);
        Task<Branch> AddBranchAsync(Branch branch);
        Task<Branch> UpdateBranchAsync(Branch branch);
        Task DeleteBranchAsync(long id);
        Task<IEnumerable<BranchMenu>> GetAllBranchMenusAsync(
            long? branchId,
            long? menuPresetItemId,
            long? menuPresetId,
            bool? availability,
            int pageIndex,
            int pageSize);
        Task<BranchMenu?> GetBranchMenuByIdAsync(long id);
        Task AssignMenuPresetToBranchAsync(long branchId, long menuPresetId);
        Task<BranchMenu> UpdateBranchMenuAvailabilityAsync(long branchMenuId, bool availability);
    }
}
