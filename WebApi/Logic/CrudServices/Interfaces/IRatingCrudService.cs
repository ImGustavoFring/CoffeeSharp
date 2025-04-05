using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.CrudServices.Interfaces
{
    public interface IRatingCrudService
    {
        Task<IEnumerable<Rating>> GetAllRatingsAsync();
        Task<Rating?> GetRatingByIdAsync(long id);
        Task<Rating> AddRatingAsync(Rating rating);
        Task<Rating> UpdateRatingAsync(Rating rating);
        Task DeleteRatingAsync(long id);
    }
}
