using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using Microsoft.Extensions.Logging;

namespace WebApi.Infrastructure.Data
{
    public class ServiceSeeder
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ServiceSeeder> _logger;

        public ServiceSeeder(IUnitOfWork unitOfWork, ILogger<ServiceSeeder> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            _logger.LogInformation("Starting seeding process.");

            var categoryCoffee = await _unitOfWork.Categories.AddAsync(new Category { Name = "Coffee" });
            var categoryTea = await _unitOfWork.Categories.AddAsync(new Category { Name = "Tea" });
            _logger.LogInformation("Categories created successfully.");

            var roleManager = await _unitOfWork.EmployeeRoles.AddAsync(new EmployeeRole { Name = "Manager" });
            var roleStaff = await _unitOfWork.EmployeeRoles.AddAsync(new EmployeeRole { Name = "Staff" });
            _logger.LogInformation("Employee roles created successfully.");

            var ratingGood = await _unitOfWork.Ratings.AddAsync(new Rating { Name = "Good", Value = 5 });
            var ratingAverage = await _unitOfWork.Ratings.AddAsync(new Rating { Name = "Average", Value = 3 });
            _logger.LogInformation("Ratings created successfully.");

            var statusPending = await _unitOfWork.BalanceHistoryStatuses.AddAsync(new BalanceHistoryStatus { Name = "Pending" });
            var statusCompleted = await _unitOfWork.BalanceHistoryStatuses.AddAsync(new BalanceHistoryStatus { Name = "Completed" });
            _logger.LogInformation("Balance history statuses created successfully.");

            var admin = await _unitOfWork.Admins.AddAsync(new Admin
            {
                UserName = "admin123",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
            });
            _logger.LogInformation("Admin created successfully.");

            var branchMain = await _unitOfWork.Branches.AddAsync(new Branch
            {
                Name = "Main Branch",
                Address = "123 Main St"
            });
            var branchSecondary = await _unitOfWork.Branches.AddAsync(new Branch
            {
                Name = "Secondary Branch",
                Address = "456 Secondary St"
            });
            _logger.LogInformation("Branches created successfully.");

            var clientJohn = await _unitOfWork.Clients.AddAsync(new Client
            {
                TelegramId = "123456789",
                Name = "John Doe",
                Balance = 100m
            });
            var clientJane = await _unitOfWork.Clients.AddAsync(new Client
            {
                TelegramId = "987654321",
                Name = "Jane Smith",
                Balance = 150m
            });
            _logger.LogInformation("Clients created successfully.");

            var menuPresetMorning = await _unitOfWork.MenuPresets.AddAsync(new MenuPreset
            {
                Name = "Morning Set",
                Description = "Ideal for mornings"
            });
            var menuPresetEvening = await _unitOfWork.MenuPresets.AddAsync(new MenuPreset
            {
                Name = "Evening Set",
                Description = "Perfect for evenings"
            });
            _logger.LogInformation("Menu presets created successfully.");

            var productEspresso = await _unitOfWork.Products.AddAsync(new Product
            {
                Name = "Espresso",
                Description = "Strong coffee shot",
                Price = 2.5m,
                CategoryId = categoryCoffee.Id
            });
            var productLatte = await _unitOfWork.Products.AddAsync(new Product
            {
                Name = "Latte",
                Description = "Coffee with milk",
                Price = 3.5m,
                CategoryId = categoryCoffee.Id
            });
            var productGreenTea = await _unitOfWork.Products.AddAsync(new Product
            {
                Name = "Green Tea",
                Description = "Refreshing green tea",
                Price = 1.8m,
                CategoryId = categoryTea.Id
            });
            _logger.LogInformation("Products created successfully.");

            var menuPresetItemEspresso = await _unitOfWork.MenuPresetItems.AddAsync(new MenuPresetItem
            {
                ProductId = productEspresso.Id,
                MenuPresetId = menuPresetMorning.Id
            });
            var menuPresetItemLatte = await _unitOfWork.MenuPresetItems.AddAsync(new MenuPresetItem
            {
                ProductId = productLatte.Id,
                MenuPresetId = menuPresetEvening.Id
            });
            _logger.LogInformation("Menu preset items created successfully.");

            var employeeAlice = await _unitOfWork.Employees.AddAsync(new Employee
            {
                Name = "Alice",
                UserName = "alice123",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("alice123"),
                RoleId = roleManager.Id,
                BranchId = branchMain.Id
            });
            var employeeBob = await _unitOfWork.Employees.AddAsync(new Employee
            {
                Name = "Bob",
                UserName = "bobux123",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("bobux123"),
                RoleId = roleStaff.Id,
                BranchId = branchSecondary.Id
            });
            _logger.LogInformation("Employees created successfully.");

            await _unitOfWork.BranchMenus.AddAsync(new BranchMenu
            {
                MenuPresetItemId = menuPresetItemEspresso.Id,
                BranchId = branchMain.Id,
                Availability = true
            });
            await _unitOfWork.BranchMenus.AddAsync(new BranchMenu
            {
                MenuPresetItemId = menuPresetItemLatte.Id,
                BranchId = branchSecondary.Id,
                Availability = true
            });
            _logger.LogInformation("Branch menus created successfully.");

            var order1 = await _unitOfWork.Orders.AddAsync(new Order
            {
                ClientId = clientJohn.Id,
                BranchId = branchMain.Id,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = DateTime.UtcNow.AddMinutes(30),
                ClientNote = "Please deliver quickly."
            });
            var order2 = await _unitOfWork.Orders.AddAsync(new Order
            {
                ClientId = clientJane.Id,
                BranchId = branchSecondary.Id,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = DateTime.UtcNow.AddMinutes(45),
                ClientNote = "Extra sugar, please."
            });
            _logger.LogInformation("Orders created successfully.");

            await _unitOfWork.OrderItems.AddAsync(new OrderItem
            {
                OrderId = order1.Id,
                ProductId = productEspresso.Id,
                EmployeeId = employeeAlice.Id,
                Price = productEspresso.Price,
                Count = 2,
                StartedAt = DateTime.UtcNow,
                DoneAt = DateTime.UtcNow.AddMinutes(5)
            });
            await _unitOfWork.OrderItems.AddAsync(new OrderItem
            {
                OrderId = order2.Id,
                ProductId = productGreenTea.Id,
                Price = productGreenTea.Price,
                Count = 1,
                StartedAt = DateTime.UtcNow,
                DoneAt = DateTime.UtcNow.AddMinutes(7)
            });
            _logger.LogInformation("Order items created successfully.");

            await _unitOfWork.BalanceHistories.AddAsync(new BalanceHistory
            {
                ClientId = clientJohn.Id,
                Sum = 50m,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow.AddMinutes(15),
                BalanceHistoryStatusId = statusCompleted.Id
            });
            await _unitOfWork.BalanceHistories.AddAsync(new BalanceHistory
            {
                ClientId = clientJane.Id,
                Sum = -20m,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow.AddMinutes(10),
                BalanceHistoryStatusId = statusPending.Id
            });
            _logger.LogInformation("Balance histories created successfully.");

            await _unitOfWork.Feedbacks.AddAsync(new Feedback
            {
                Content = "Great service!",
                RatingId = ratingGood.Id,
                OrderId = order1.Id
            });
            await _unitOfWork.Feedbacks.AddAsync(new Feedback
            {
                Content = "Average experience.",
                RatingId = ratingAverage.Id,
                OrderId = order2.Id
            });
            _logger.LogInformation("Feedbacks created successfully.");

            _logger.LogInformation("Displaying contents of all tables:");

            var allCategories = await _unitOfWork.Categories.GetAllAsync();
            foreach (var category in allCategories)
                _logger.LogInformation("Category: {Id} - {Name}", category.Id, category.Name);

            var allEmployeeRoles = await _unitOfWork.EmployeeRoles.GetAllAsync();
            foreach (var role in allEmployeeRoles)
                _logger.LogInformation("EmployeeRole: {Id} - {Name}", role.Id, role.Name);

            var allRatings = await _unitOfWork.Ratings.GetAllAsync();
            foreach (var rating in allRatings)
                _logger.LogInformation("Rating: {Id} - {Name} ({Value})", rating.Id, rating.Name, rating.Value);

            var allBalanceHistoryStatuses = await _unitOfWork.BalanceHistoryStatuses.GetAllAsync();
            foreach (var status in allBalanceHistoryStatuses)
                _logger.LogInformation("BalanceHistoryStatus: {Id} - {Name}", status.Id, status.Name);

            var allAdmins = await _unitOfWork.Admins.GetAllAsync();
            foreach (var a in allAdmins)
                _logger.LogInformation("Admin: {Id} - {UserName}", a.Id, a.UserName);

            var allBranches = await _unitOfWork.Branches.GetAllAsync();
            foreach (var branch in allBranches)
                _logger.LogInformation("Branch: {Id} - {Name} ({Address})", branch.Id, branch.Name, branch.Address);

            var allClients = await _unitOfWork.Clients.GetAllAsync();
            foreach (var client in allClients)
                _logger.LogInformation("Client: {Id} - {Name} (TelegramId: {TelegramId}, Balance: {Balance})",
                    client.Id, client.Name, client.TelegramId, client.Balance);

            var allMenuPresets = await _unitOfWork.MenuPresets.GetAllAsync();
            foreach (var preset in allMenuPresets)
                _logger.LogInformation("MenuPreset: {Id} - {Name} ({Description})", preset.Id, preset.Name, preset.Description);

            var allProducts = await _unitOfWork.Products.GetAllAsync();
            foreach (var product in allProducts)
                _logger.LogInformation("Product: {Id} - {Name} ({Description}, Price: {Price}, CategoryId: {CategoryId})",
                    product.Id, product.Name, product.Description, product.Price, product.CategoryId);

            var allMenuPresetItems = await _unitOfWork.MenuPresetItems.GetAllAsync();
            foreach (var item in allMenuPresetItems)
                _logger.LogInformation("MenuPresetItem: {Id} - ProductId: {ProductId}, MenuPresetId: {MenuPresetId}",
                    item.Id, item.ProductId, item.MenuPresetId);

            var allEmployees = await _unitOfWork.Employees.GetAllAsync();
            foreach (var emp in allEmployees)
                _logger.LogInformation("Employee: {Id} - {Name} ({UserName}, RoleId: {RoleId}, BranchId: {BranchId})",
                    emp.Id, emp.Name, emp.UserName, emp.RoleId, emp.BranchId);

            var allBranchMenus = await _unitOfWork.BranchMenus.GetAllAsync();
            foreach (var bm in allBranchMenus)
                _logger.LogInformation("BranchMenu: {Id} - MenuPresetItemId: {MenuPresetItemId}, BranchId: {BranchId}, Availability: {Availability}",
                    bm.Id, bm.MenuPresetItemId, bm.BranchId, bm.Availability);

            var allOrders = await _unitOfWork.Orders.GetAllAsync();
            foreach (var order in allOrders)
                _logger.LogInformation("Order: {Id} - ClientId: {ClientId}, BranchId: {BranchId}, CreatedAt: {CreatedAt}",
                    order.Id, order.ClientId, order.BranchId, order.CreatedAt);

            var allOrderItems = await _unitOfWork.OrderItems.GetAllAsync();
            foreach (var orderItem in allOrderItems)
                _logger.LogInformation("OrderItem: {Id} - OrderId: {OrderId}, ProductId: {ProductId}, EmployeeId: {EmployeeId}, Count: {Count}",
                    orderItem.Id, orderItem.OrderId, orderItem.ProductId, orderItem.EmployeeId, orderItem.Count);

            var allBalanceHistories = await _unitOfWork.BalanceHistories.GetAllAsync();
            foreach (var bh in allBalanceHistories)
                _logger.LogInformation("BalanceHistory: {Id} - ClientId: {ClientId}, Sum: {Sum}, StatusId: {StatusId}",
                    bh.Id, bh.ClientId, bh.Sum, bh.BalanceHistoryStatusId);

            var allFeedbacks = await _unitOfWork.Feedbacks.GetAllAsync();
            foreach (var feedback in allFeedbacks)
                _logger.LogInformation("Feedback: {Id} - {Content} (RatingId: {RatingId}, OrderId: {OrderId})",
                    feedback.Id, feedback.Content, feedback.RatingId, feedback.OrderId);

            _logger.LogInformation("Seeding process completed successfully.");
        }
    }
}
