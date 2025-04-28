using System.Linq.Expressions;
using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Extensions;
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

        public async Task<(IEnumerable<Rating> Items, int TotalCount)> GetAllRatingsAsync(
            string? name = null,
            long? value = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<Rating, bool>> filter = r => true;

            if (!string.IsNullOrWhiteSpace(name))
                filter = filter.AndAlso(r => r.Name.Contains(name));

            if (value.HasValue)
                filter = filter.AndAlso(r => r.Value == value.Value);

            var total = await _unitOfWork.Ratings.CountAsync(filter);

            var items = await _unitOfWork.Ratings.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(r => r.Value).ThenBy(r => r.Name),
                includes: null,
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (items, total);
        }

        public async Task<Rating?> GetRatingByIdAsync(long id)
        {
            return await _unitOfWork.Ratings.GetByIdAsync(id);
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            var existing = await _unitOfWork.Ratings.GetOneAsync(r => r.Value == rating.Value);

            if (existing != null)
            {
                throw new InvalidOperationException($"Rating with value '{rating.Value}' already exists.");
            }

            var result = await _unitOfWork.Ratings.AddOneAsync(rating);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Rating> UpdateRatingAsync(Rating rating)
        {
            var existing = await _unitOfWork.Ratings.GetByIdAsync(rating.Id);

            if (existing == null)
            {
                throw new ArgumentException("Rating not found.");
            }

            existing.Name = rating.Name;
            existing.Value = rating.Value;

            _unitOfWork.Ratings.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteRatingAsync(long id)
        {
            await _unitOfWork.Ratings.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<(IEnumerable<EmployeeRole> Items, int TotalCount)> GetAllEmployeeRolesAsync(
            string? nameFilter = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<EmployeeRole, bool>> filter = r => true;

            if (!string.IsNullOrWhiteSpace(nameFilter))
                filter = filter.AndAlso(r => r.Name.Contains(nameFilter));

            var total = await _unitOfWork.EmployeeRoles.CountAsync(filter);

            var items = await _unitOfWork.EmployeeRoles.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(r => r.Name),
                includes: null,
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (items, total);
        }

        public async Task<EmployeeRole?> GetEmployeeRoleByIdAsync(long id)
        {
            return await _unitOfWork.EmployeeRoles.GetByIdAsync(id);
        }

        public async Task<EmployeeRole> AddEmployeeRoleAsync(EmployeeRole role)
        {
            var existing = await _unitOfWork.EmployeeRoles.GetOneAsync(r => r.Name.ToLower() == role.Name.ToLower());

            if (existing != null)
            {
                throw new InvalidOperationException($"EmployeeRole with name '{role.Name}' already exists.");
            }

            var result = await _unitOfWork.EmployeeRoles.AddOneAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<EmployeeRole> UpdateEmployeeRoleAsync(EmployeeRole role)
        {
            var existing = await _unitOfWork.EmployeeRoles.GetByIdAsync(role.Id);

            if (existing == null)
            {
                throw new ArgumentException("EmployeeRole not found.");
            }

            existing.Name = role.Name;

            _unitOfWork.EmployeeRoles.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteEmployeeRoleAsync(long id)
        {
            await _unitOfWork.EmployeeRoles.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<(IEnumerable<BalanceHistoryStatus> Items, int TotalCount)> GetAllBalanceHistoryStatusesAsync(
            string? nameFilter = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<BalanceHistoryStatus, bool>> filter = s => true;

            if (!string.IsNullOrWhiteSpace(nameFilter))
                filter = filter.AndAlso(s => s.Name.Contains(nameFilter));

            var total = await _unitOfWork.BalanceHistoryStatuses.CountAsync(filter);

            var items = await _unitOfWork.BalanceHistoryStatuses.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(s => s.Name),
                includes: null,
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (items, total);
        }

        public async Task<BalanceHistoryStatus?> GetBalanceHistoryStatusByIdAsync(long id)
        {
            return await _unitOfWork.BalanceHistoryStatuses.GetByIdAsync(id);
        }

        public async Task<BalanceHistoryStatus> AddBalanceHistoryStatusAsync(BalanceHistoryStatus status)
        {
            var existing = await _unitOfWork.BalanceHistoryStatuses.GetOneAsync(s => s.Name == status.Name);

            if (existing != null)
            {
                throw new InvalidOperationException($"BalanceHistoryStatus with name '{status.Name}' already exists.");
            }

            var result = await _unitOfWork.BalanceHistoryStatuses.AddOneAsync(status);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<BalanceHistoryStatus> UpdateBalanceHistoryStatusAsync(BalanceHistoryStatus status)
        {
            var existing = await _unitOfWork.BalanceHistoryStatuses.GetByIdAsync(status.Id);

            if (existing == null)
            {
                throw new ArgumentException("BalanceHistoryStatus not found.");
            }

            existing.Name = status.Name;

            _unitOfWork.BalanceHistoryStatuses.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteBalanceHistoryStatusAsync(long id)
        {
            await _unitOfWork.BalanceHistoryStatuses.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
