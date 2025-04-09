using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.CrudServices.Interfaces
{
    public interface IFeedbackCrudService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback?> GetFeedbackByIdAsync(long id);
        Task<Feedback> AddFeedbackAsync(Feedback feedback);
        Task<Feedback> UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(long id);
    }
}
