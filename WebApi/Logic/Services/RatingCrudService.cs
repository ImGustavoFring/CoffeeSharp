using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class RatingCrudService : IRatingCrudService
    {
        private readonly IRepository<Rating> _repository;

        public RatingCrudService(IRepository<Rating> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Rating>> GetAllRatingsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Rating?> GetRatingByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            return await _repository.AddAsync(rating);
        }

        public async Task<Rating> UpdateRatingAsync(Rating rating)
        {
            return await _repository.UpdateAsync(rating);
        }

        public async Task DeleteRatingAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
