using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IBranchMenuCrudService
    {
        Task<IEnumerable<BranchMenu>> GetAllBranchMenusAsync();
        Task<BranchMenu?> GetBranchMenuByIdAsync(long id);
        Task<BranchMenu> AddBranchMenuAsync(BranchMenu menu);
        Task<BranchMenu> UpdateBranchMenuAsync(BranchMenu menu);
        Task DeleteBranchMenuAsync(long id);
    }
}
