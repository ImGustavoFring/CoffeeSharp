using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.CrudServices.Interfaces
{
    public interface IBranchCrudService
    {
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task<Branch?> GetBranchByIdAsync(long id);
        Task<Branch> AddBranchAsync(Branch branch);
        Task<Branch> UpdateBranchAsync(Branch branch);
        Task DeleteBranchAsync(long id);
    }
}
