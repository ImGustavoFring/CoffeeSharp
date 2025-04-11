using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
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

        public async Task<Feedback?> GetFeedbackByIdAsync(long id)
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

        public async Task DeleteFeedbackAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
