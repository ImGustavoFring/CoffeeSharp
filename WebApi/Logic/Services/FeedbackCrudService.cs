using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class FeedbackCrudService : IFeedbackCrudService
    {
        private readonly IRepository<Feedback> _repository;

        public FeedbackCrudService(IRepository<Feedback> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Feedback> AddFeedbackAsync(Feedback feedback)
        {
            return await _repository.AddAsync(feedback);
        }

        public async Task<Feedback> UpdateFeedbackAsync(Feedback feedback)
        {
            return await _repository.UpdateAsync(feedback);
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
