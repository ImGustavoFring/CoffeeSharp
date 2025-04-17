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

            var categoryCoffee = await _unitOfWork.Categories.AddOneAsync(new Category { Name = "Coffee" });
            var categoryTea = await _unitOfWork.Categories.AddOneAsync(new Category { Name = "Tea" });
            _logger.LogInformation("Categories created successfully.");

            var roleManager = await _unitOfWork.EmployeeRoles.AddOneAsync(new EmployeeRole { Name = "Manager" });
            var roleStaff = await _unitOfWork.EmployeeRoles.AddOneAsync(new EmployeeRole { Name = "Staff" });
            _logger.LogInformation("Employee roles created successfully.");

            var ratingGood = await _unitOfWork.Ratings.AddOneAsync(new Rating { Name = "Good", Value = 5 });
            var ratingAverage = await _unitOfWork.Ratings.AddOneAsync(new Rating { Name = "Average", Value = 3 });
            _logger.LogInformation("Ratings created successfully.");

            var statusCompleted = await _unitOfWork.BalanceHistoryStatuses.AddOneAsync(new BalanceHistoryStatus { Name = "Completed" });
            var statusCancelled = await _unitOfWork.BalanceHistoryStatuses.AddOneAsync(new BalanceHistoryStatus { Name = "Cancelled" });
            var statusPending = await _unitOfWork.BalanceHistoryStatuses.AddOneAsync(new BalanceHistoryStatus { Name = "Pending" });
            _logger.LogInformation("Balance history statuses created successfully.");

            var admin = await _unitOfWork.Admins.AddOneAsync(new Admin
            {
                UserName = "admin123",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
            });
            _logger.LogInformation("Admin created successfully.");

            var branchMain = await _unitOfWork.Branches.AddOneAsync(new Branch
            {
                Name = "Main Branch",
                Address = "123 Main St"
            });
            var branchSecondary = await _unitOfWork.Branches.AddOneAsync(new Branch
            {
                Name = "Secondary Branch",
                Address = "456 Secondary St"
            });
            _logger.LogInformation("Branches created successfully.");

            var clientJohn = await _unitOfWork.Clients.AddOneAsync(new Client
            {
                TelegramId = "123456789",
                Name = "John Doe",
                Balance = 100m
            });
            var clientJane = await _unitOfWork.Clients.AddOneAsync(new Client
            {
                TelegramId = "987654321",
                Name = "Jane Smith",
                Balance = 150m
            });
            _logger.LogInformation("Clients created successfully.");

            var menuPresetMorning = await _unitOfWork.MenuPresets.AddOneAsync(new MenuPreset
            {
                Name = "Morning Set",
                Description = "Ideal for mornings"
            });
            var menuPresetEvening = await _unitOfWork.MenuPresets.AddOneAsync(new MenuPreset
            {
                Name = "Evening Set",
                Description = "Perfect for evenings"
            });
            _logger.LogInformation("Menu presets created successfully.");

            var productEspresso = await _unitOfWork.Products.AddOneAsync(new Product
            {
                Name = "Espresso",
                Description = "Strong coffee shot",
                Price = 2.5m,
                Category = categoryCoffee
            });
            var productLatte = await _unitOfWork.Products.AddOneAsync(new Product
            {
                Name = "Latte",
                Description = "Coffee with milk",
                Price = 3.5m,
                Category = categoryCoffee
            });
            var productGreenTea = await _unitOfWork.Products.AddOneAsync(new Product
            {
                Name = "Green Tea",
                Description = "Refreshing green tea",
                Price = 1.8m,
                Category = categoryTea
            });
            _logger.LogInformation("Products created successfully.");

            var menuPresetItemEspresso = await _unitOfWork.MenuPresetItems.AddOneAsync(new MenuPresetItem
            {
                Product = productEspresso,
                MenuPreset = menuPresetMorning
            });
            var menuPresetItemLatte = await _unitOfWork.MenuPresetItems.AddOneAsync(new MenuPresetItem
            {
                Product = productLatte,
                MenuPreset = menuPresetEvening
            });
            _logger.LogInformation("Menu preset items created successfully.");

            var employeeAlice = await _unitOfWork.Employees.AddOneAsync(new Employee
            {
                Name = "Alice",
                UserName = "alice123",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("alice123"),
                Role = roleManager,
                Branch = branchMain
            });
            var employeeBob = await _unitOfWork.Employees.AddOneAsync(new Employee
            {
                Name = "Bob",
                UserName = "bobux123",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("bobux123"),
                Role = roleStaff,
                Branch = branchSecondary
            });
            _logger.LogInformation("Employees created successfully.");

            await _unitOfWork.BranchMenus.AddOneAsync(new BranchMenu
            {
                MenuPresetItem = menuPresetItemEspresso,
                Branch = branchMain,
                Availability = true
            });
            await _unitOfWork.BranchMenus.AddOneAsync(new BranchMenu
            {
                MenuPresetItem = menuPresetItemLatte,
                Branch = branchSecondary,
                Availability = true
            });
            _logger.LogInformation("Branch menus created successfully.");

            var order1 = await _unitOfWork.Orders.AddOneAsync(new Order
            {
                Client = clientJohn,
                Branch = branchMain,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = DateTime.UtcNow.AddMinutes(30),
                ClientNote = "Please deliver quickly."
            });
            var order2 = await _unitOfWork.Orders.AddOneAsync(new Order
            {
                Client = clientJane,
                Branch = branchSecondary,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = DateTime.UtcNow.AddMinutes(45),
                ClientNote = "Extra sugar, please."
            });
            _logger.LogInformation("Orders created successfully.");

            await _unitOfWork.OrderItems.AddOneAsync(new OrderItem
            {
                Order = order1,
                Product = productEspresso,
                Employee = employeeAlice,
                Price = productEspresso.Price,
                Count = 2,
                StartedAt = DateTime.UtcNow
            });
            await _unitOfWork.OrderItems.AddOneAsync(new OrderItem
            {
                Order = order2,
                Product = productGreenTea,
                Price = productGreenTea.Price,
                Count = 1,
                StartedAt = DateTime.UtcNow
            });
            _logger.LogInformation("Order items created successfully.");

            await _unitOfWork.BalanceHistories.AddOneAsync(new BalanceHistory
            {
                Client = clientJohn,
                Sum = 50m,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow.AddMinutes(15),
                BalanceHistoryStatus = statusCompleted
            });
            await _unitOfWork.BalanceHistories.AddOneAsync(new BalanceHistory
            {
                Client = clientJane,
                Sum = -20m,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow.AddMinutes(10),
                BalanceHistoryStatus = statusPending
            });
            _logger.LogInformation("Balance histories created successfully.");

            await _unitOfWork.Feedbacks.AddOneAsync(new Feedback
            {
                Content = "Great service!",
                Rating = ratingGood,
                Order = order1
            });
            await _unitOfWork.Feedbacks.AddOneAsync(new Feedback
            {
                Content = "Average experience.",
                Rating = ratingAverage,
                Order = order2
            });

            _logger.LogInformation("Feedbacks created successfully.");

            _logger.LogInformation("Displaying contents of all tables:");

            await _unitOfWork.SaveChangesAsync();

            var allCategories = await _unitOfWork.Categories.GetManyAsync();

            foreach (var category in allCategories)
                _logger.LogInformation("Category: {Id} - {Name}", category.Id, category.Name);

            var allEmployeeRoles = await _unitOfWork.EmployeeRoles.GetManyAsync();
            foreach (var role in allEmployeeRoles)
                _logger.LogInformation("EmployeeRole: {Id} - {Name}", role.Id, role.Name);

            var allRatings = await _unitOfWork.Ratings.GetManyAsync();
            foreach (var rating in allRatings)
                _logger.LogInformation("Rating: {Id} - {Name} ({Value})", rating.Id, rating.Name, rating.Value);

            var allBalanceHistoryStatuses = await _unitOfWork.BalanceHistoryStatuses.GetManyAsync();
            foreach (var status in allBalanceHistoryStatuses)
                _logger.LogInformation("BalanceHistoryStatus: {Id} - {Name}", status.Id, status.Name);

            var allAdmins = await _unitOfWork.Admins.GetManyAsync();
            foreach (var a in allAdmins)
                _logger.LogInformation("Admin: {Id} - {UserName}", a.Id, a.UserName);

            var allBranches = await _unitOfWork.Branches.GetManyAsync();
            foreach (var branch in allBranches)
                _logger.LogInformation("Branch: {Id} - {Name} ({Address})", branch.Id, branch.Name, branch.Address);

            var allClients = await _unitOfWork.Clients.GetManyAsync();
            foreach (var client in allClients)
                _logger.LogInformation("Client: {Id} - {Name} (TelegramId: {TelegramId}, Balance: {Balance})",
                    client.Id, client.Name, client.TelegramId, client.Balance);

            var allMenuPresets = await _unitOfWork.MenuPresets.GetManyAsync();
            foreach (var preset in allMenuPresets)
                _logger.LogInformation("MenuPreset: {Id} - {Name} ({Description})", preset.Id, preset.Name, preset.Description);

            var allProducts = await _unitOfWork.Products.GetManyAsync();
            foreach (var product in allProducts)
                _logger.LogInformation("Product: {Id} - {Name} ({Description}, Price: {Price}, CategoryId: {CategoryId})",
                    product.Id, product.Name, product.Description, product.Price, product.CategoryId);

            var allMenuPresetItems = await _unitOfWork.MenuPresetItems.GetManyAsync();
            foreach (var item in allMenuPresetItems)
                _logger.LogInformation("MenuPresetItem: {Id} - ProductId: {ProductId}, MenuPresetId: {MenuPresetId}",
                    item.Id, item.ProductId, item.MenuPresetId);

            var allEmployees = await _unitOfWork.Employees.GetManyAsync();
            foreach (var emp in allEmployees)
                _logger.LogInformation("Employee: {Id} - {Name} ({UserName}, RoleId: {RoleId}, BranchId: {BranchId})",
                    emp.Id, emp.Name, emp.UserName, emp.RoleId, emp.BranchId);

            var allBranchMenus = await _unitOfWork.BranchMenus.GetManyAsync();
            foreach (var bm in allBranchMenus)
                _logger.LogInformation("BranchMenu: {Id} - MenuPresetItemId: {MenuPresetItemId}, BranchId: {BranchId}, Availability: {Availability}",
                    bm.Id, bm.MenuPresetItemId, bm.BranchId, bm.Availability);

            var allOrders = await _unitOfWork.Orders.GetManyAsync();
            foreach (var order in allOrders)
                _logger.LogInformation("Order: {Id} - ClientId: {ClientId}, BranchId: {BranchId}, CreatedAt: {CreatedAt}",
                    order.Id, order.ClientId, order.BranchId, order.CreatedAt);

            var allOrderItems = await _unitOfWork.OrderItems.GetManyAsync();
            foreach (var orderItem in allOrderItems)
                _logger.LogInformation("OrderItem: {Id} - OrderId: {OrderId}, ProductId: {ProductId}, EmployeeId: {EmployeeId}, Count: {Count}",
                    orderItem.Id, orderItem.OrderId, orderItem.ProductId, orderItem.EmployeeId, orderItem.Count);

            var allBalanceHistories = await _unitOfWork.BalanceHistories.GetManyAsync();
            foreach (var bh in allBalanceHistories)
                _logger.LogInformation("BalanceHistory: {Id} - ClientId: {ClientId}, Sum: {Sum}, StatusId: {StatusId}",
                    bh.Id, bh.ClientId, bh.Sum, bh.BalanceHistoryStatusId);

            var allFeedbacks = await _unitOfWork.Feedbacks.GetManyAsync();
            foreach (var feedback in allFeedbacks)
                _logger.LogInformation("Feedback: {Id} - {Content} (RatingId: {RatingId}, OrderId: {OrderId})",
                    feedback.Id, feedback.Content, feedback.RatingId, feedback.OrderId);

            _logger.LogInformation("Seeding process completed successfully.");
        }
    }
}
