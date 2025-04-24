using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;

namespace WebApi.Infrastructure.UnitsOfWorks.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<BalanceHistory> BalanceHistories { get; }
        IRepository<BalanceHistoryStatus> BalanceHistoryStatuses { get; }
        IRepository<Branch> Branches { get; }
        IRepository<BranchMenu> BranchMenus { get; }
        IRepository<Category> Categories { get; }
        IRepository<Client> Clients { get; }
        IRepository<Employee> Employees { get; }
        IRepository<EmployeeRole> EmployeeRoles { get; }
        IRepository<Feedback> Feedbacks { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<Product> Products { get; }
        IRepository<Rating> Ratings { get; }
        IRepository<Admin> Admins { get; }
        IRepository<MenuPreset> MenuPresets { get; }
        IRepository<MenuPresetItem> MenuPresetItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
