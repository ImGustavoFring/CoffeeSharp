using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReferenceDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Rating>> GetAllRatingsAsync()
        {
            return await _unitOfWork.Ratings.GetAllAsync();
        }

        public async Task<Rating?> GetRatingByIdAsync(long id)
        {
            return await _unitOfWork.Ratings.GetByIdAsync(id);
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            var existing = await _unitOfWork.Ratings.GetSingleAsync(r => r.Value == rating.Value);
            if (existing != null)
                throw new InvalidOperationException($"Rating with value '{rating.Value}' already exists.");

            return await _unitOfWork.Ratings.AddAsync(rating);
        }

        public async Task<Rating> UpdateRatingAsync(Rating rating)
        {
            var existing = await _unitOfWork.Ratings.GetByIdAsync(rating.Id);
            if (existing == null)
                throw new ArgumentException("Rating not found.");
            existing.Name = rating.Name;
            existing.Value = rating.Value;
            return await _unitOfWork.Ratings.UpdateAsync(existing);
        }

        public async Task DeleteRatingAsync(long id)
        {
            await _unitOfWork.Ratings.DeleteAsync(id);
        }

        public async Task<IEnumerable<EmployeeRole>> GetAllEmployeeRolesAsync()
        {
            return await _unitOfWork.EmployeeRoles.GetAllAsync();
        }

        public async Task<EmployeeRole?> GetEmployeeRoleByIdAsync(long id)
        {
            return await _unitOfWork.EmployeeRoles.GetByIdAsync(id);
        }

        public async Task<EmployeeRole> AddEmployeeRoleAsync(EmployeeRole role)
        {
            var existing = await _unitOfWork.EmployeeRoles.GetSingleAsync(r => r.Name.ToLower() == role.Name.ToLower());
            if (existing != null)
                throw new InvalidOperationException($"EmployeeRole with name '{role.Name}' already exists.");

            return await _unitOfWork.EmployeeRoles.AddAsync(role);
        }

        public async Task<EmployeeRole> UpdateEmployeeRoleAsync(EmployeeRole role)
        {
            var existing = await _unitOfWork.EmployeeRoles.GetByIdAsync(role.Id);
            if (existing == null)
                throw new ArgumentException("EmployeeRole not found.");
            existing.Name = role.Name;
            return await _unitOfWork.EmployeeRoles.UpdateAsync(existing);
        }

        public async Task DeleteEmployeeRoleAsync(long id)
        {
            await _unitOfWork.EmployeeRoles.DeleteAsync(id);
        }

        public async Task<IEnumerable<BalanceHistoryStatus>> GetAllBalanceHistoryStatusesAsync()
        {
            return await _unitOfWork.BalanceHistoryStatuses.GetAllAsync();
        }

        public async Task<BalanceHistoryStatus?> GetBalanceHistoryStatusByIdAsync(long id)
        {
            return await _unitOfWork.BalanceHistoryStatuses.GetByIdAsync(id);
        }

        public async Task<BalanceHistoryStatus> AddBalanceHistoryStatusAsync(BalanceHistoryStatus status)
        {
            var existing = await _unitOfWork.BalanceHistoryStatuses.GetSingleAsync(s => s.Name == status.Name);
            if (existing != null)
                throw new InvalidOperationException($"BalanceHistoryStatus with name '{status.Name}' already exists.");

            return await _unitOfWork.BalanceHistoryStatuses.AddAsync(status);
        }

        public async Task<BalanceHistoryStatus> UpdateBalanceHistoryStatusAsync(BalanceHistoryStatus status)
        {
            var existing = await _unitOfWork.BalanceHistoryStatuses.GetByIdAsync(status.Id);
            if (existing == null)
                throw new ArgumentException("BalanceHistoryStatus not found.");
            existing.Name = status.Name;
            return await _unitOfWork.BalanceHistoryStatuses.UpdateAsync(existing);
        }

        public async Task DeleteBalanceHistoryStatusAsync(long id)
        {
            await _unitOfWork.BalanceHistoryStatuses.DeleteAsync(id);
        }
    }
}
