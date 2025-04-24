using CoffeeSharp.Domain.Entities;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Domain.Entities;
using WebApi.Infrastructure.Data;
using WebApi.Infrastructure.Repositories;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;

namespace WebApi.Infrastructure.UnitsOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeSharpDbContext _context;

        public IRepository<BalanceHistory> BalanceHistories { get; }
        public IRepository<BalanceHistoryStatus> BalanceHistoryStatuses { get; }
        public IRepository<Branch> Branches { get; }
        public IRepository<BranchMenu> BranchMenus { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Client> Clients { get; }
        public IRepository<Employee> Employees { get; }
        public IRepository<EmployeeRole> EmployeeRoles { get; }
        public IRepository<Feedback> Feedbacks { get; }
        public IRepository<Order> Orders { get; }
        public IRepository<OrderItem> OrderItems { get; }
        public IRepository<Product> Products { get; }
        public IRepository<Rating> Ratings { get; }
        public IRepository<Admin> Admins { get; }
        public IRepository<MenuPreset> MenuPresets { get; }
        public IRepository<MenuPresetItems> MenuPresetItems { get; }

        public UnitOfWork(CoffeeSharpDbContext context)
        {
            _context = context;

            BalanceHistories = new Repository<BalanceHistory>(_context);
            BalanceHistoryStatuses = new Repository<BalanceHistoryStatus>(_context);
            Branches = new Repository<Branch>(_context);
            BranchMenus = new Repository<BranchMenu>(_context);
            Categories = new Repository<Category>(_context);
            Clients = new Repository<Client>(_context);
            Employees = new Repository<Employee>(_context);
            EmployeeRoles = new Repository<EmployeeRole>(_context);
            Feedbacks = new Repository<Feedback>(_context);
            Orders = new Repository<Order>(_context);
            OrderItems = new Repository<OrderItem>(_context);
            Products = new Repository<Product>(_context);
            Ratings = new Repository<Rating>(_context);
            Admins = new Repository<Admin>(_context);
            MenuPresets = new Repository<MenuPreset>(_context);
            MenuPresetItems = new Repository<MenuPresetItems>(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
