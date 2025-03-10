using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IBranchMenuService
    {
        Task<IEnumerable<BranchMenu>> GetAllBranchMenusAsync();
        Task<BranchMenu?> GetBranchMenuByIdAsync(int id);
        Task<BranchMenu> AddBranchMenuAsync(BranchMenu menu);
        Task<BranchMenu> UpdateBranchMenuAsync(BranchMenu menu);
        Task DeleteBranchMenuAsync(int id);
    }
}
