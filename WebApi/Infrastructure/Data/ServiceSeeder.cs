using CoffeeSharp.Domain.Entities;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Infrastructure.Data
{
    public class ServiceSeeder
    {
        private readonly ICategoryService _categoryService;
        private readonly IEmployeeRoleService _employeeRoleService;
        private readonly IRatingService _ratingService;
        private readonly IBalanceHistoryStatusService _balanceHistoryStatusService;
        private readonly IAdminService _adminService;
        private readonly IBranchService _branchService;
        private readonly IClientService _clientService;
        private readonly IMenuPresetService _menuPresetService;
        private readonly IProductService _productService;
        private readonly IMenuPresetItemService _menuPresetItemService;
        private readonly IEmployeeService _employeeService;
        private readonly IBranchMenuService _branchMenuService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IBalanceHistoryService _balanceHistoryService;
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<ServiceSeeder> _logger;

        public ServiceSeeder(
            ICategoryService categoryService,
            IEmployeeRoleService employeeRoleService,
            IRatingService ratingService,
            IBalanceHistoryStatusService balanceHistoryStatusService,
            IAdminService adminService,
            IBranchService branchService,
            IClientService clientService,
            IMenuPresetService menuPresetService,
            IProductService productService,
            IMenuPresetItemService menuPresetItemService,
            IEmployeeService employeeService,
            IBranchMenuService branchMenuService,
            IOrderService orderService,
            IOrderItemService orderItemService,
            IBalanceHistoryService balanceHistoryService,
            IFeedbackService feedbackService,
            ILogger<ServiceSeeder> logger)
        {
            _categoryService = categoryService;
            _employeeRoleService = employeeRoleService;
            _ratingService = ratingService;
            _balanceHistoryStatusService = balanceHistoryStatusService;
            _adminService = adminService;
            _branchService = branchService;
            _clientService = clientService;
            _menuPresetService = menuPresetService;
            _productService = productService;
            _menuPresetItemService = menuPresetItemService;
            _employeeService = employeeService;
            _branchMenuService = branchMenuService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _balanceHistoryService = balanceHistoryService;
            _feedbackService = feedbackService;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            _logger.LogInformation("Starting seeding process.");

            var categoryCoffee = await _categoryService.AddCategoryAsync(new Category { Name = "Coffee" });
            var categoryTea = await _categoryService.AddCategoryAsync(new Category { Name = "Tea" });
            _logger.LogInformation("Categories created successfully.");

            var roleManager = await _employeeRoleService.AddRoleAsync(new EmployeeRole { Name = "Manager" });
            var roleStaff = await _employeeRoleService.AddRoleAsync(new EmployeeRole { Name = "Staff" });
            _logger.LogInformation("Employee roles created successfully.");

            var ratingGood = await _ratingService.AddRatingAsync(new Rating { Name = "Good", Value = 5 });
            var ratingAverage = await _ratingService.AddRatingAsync(new Rating { Name = "Average", Value = 3 });
            _logger.LogInformation("Ratings created successfully.");

            var statusPending = await _balanceHistoryStatusService.AddStatusAsync(new BalanceHistoryStatus { Name = "Pending" });
            var statusCompleted = await _balanceHistoryStatusService.AddStatusAsync(new BalanceHistoryStatus { Name = "Completed" });
            _logger.LogInformation("Balance history statuses created successfully.");

            var admin = await _adminService.AddAdminAsync(new Admin
            {
                UserName = "admin",
                PasswordHash = "hashed_password"
            });
            _logger.LogInformation("Admin created successfully.");

            var branchMain = await _branchService.AddBranchAsync(new Branch
            {
                Name = "Main Branch",
                Address = "123 Main St"
            });
            var branchSecondary = await _branchService.AddBranchAsync(new Branch
            {
                Name = "Secondary Branch",
                Address = "456 Secondary St"
            });
            _logger.LogInformation("Branches created successfully.");

            var clientJohn = await _clientService.AddClientAsync(new Client
            {
                TelegramId = "123456789",
                Name = "John Doe",
                Balance = 100m
            });
            var clientJane = await _clientService.AddClientAsync(new Client
            {
                TelegramId = "987654321",
                Name = "Jane Smith",
                Balance = 150m
            });
            _logger.LogInformation("Clients created successfully.");

            var menuPresetMorning = await _menuPresetService.AddMenuPresetAsync(new MenuPreset
            {
                Name = "Morning Set",
                Description = "Ideal for mornings"
            });
            var menuPresetEvening = await _menuPresetService.AddMenuPresetAsync(new MenuPreset
            {
                Name = "Evening Set",
                Description = "Perfect for evenings"
            });
            _logger.LogInformation("Menu presets created successfully.");

            var productEspresso = await _productService.AddProductAsync(new Product
            {
                Name = "Espresso",
                Description = "Strong coffee shot",
                Price = 2.5m,
                CategoryId = categoryCoffee.Id
            });
            var productLatte = await _productService.AddProductAsync(new Product
            {
                Name = "Latte",
                Description = "Coffee with milk",
                Price = 3.5m,
                CategoryId = categoryCoffee.Id
            });
            var productGreenTea = await _productService.AddProductAsync(new Product
            {
                Name = "Green Tea",
                Description = "Refreshing green tea",
                Price = 1.8m,
                CategoryId = categoryTea.Id
            });
            _logger.LogInformation("Products created successfully.");

            var menuPresetItemEspresso = await _menuPresetItemService.AddMenuPresetItemAsync(new MenuPresetItem
            {
                ProductId = productEspresso.Id,
                MenuPresetId = menuPresetMorning.Id
            });
            var menuPresetItemLatte = await _menuPresetItemService.AddMenuPresetItemAsync(new MenuPresetItem
            {
                ProductId = productLatte.Id,
                MenuPresetId = menuPresetEvening.Id
            });
            _logger.LogInformation("Menu preset items created successfully.");

            var employeeAlice = await _employeeService.AddEmployeeAsync(new Employee
            {
                Name = "Alice",
                UserName = "alice",
                PasswordHash = "hashed_alice",
                RoleId = roleManager.Id,
                BranchId = branchMain.Id
            });
            var employeeBob = await _employeeService.AddEmployeeAsync(new Employee
            {
                Name = "Bob",
                UserName = "bob",
                PasswordHash = "hashed_bob",
                RoleId = roleStaff.Id,
                BranchId = branchSecondary.Id
            });
            _logger.LogInformation("Employees created successfully.");

            await _branchMenuService.AddBranchMenuAsync(new BranchMenu
            {
                MenuPresetItemId = menuPresetItemEspresso.Id,
                BranchId = branchMain.Id,
                Availability = true
            });
            await _branchMenuService.AddBranchMenuAsync(new BranchMenu
            {
                MenuPresetItemId = menuPresetItemLatte.Id,
                BranchId = branchSecondary.Id,
                Availability = true
            });
            _logger.LogInformation("Branch menus created successfully.");

            var order1 = await _orderService.AddOrderAsync(new Order
            {
                ClientId = clientJohn.Id,
                BranchId = branchMain.Id,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = DateTime.UtcNow.AddMinutes(30),
                ClientNote = "Please deliver quickly."
            });
            var order2 = await _orderService.AddOrderAsync(new Order
            {
                ClientId = clientJane.Id,
                BranchId = branchSecondary.Id,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = DateTime.UtcNow.AddMinutes(45),
                ClientNote = "Extra sugar, please."
            });
            _logger.LogInformation("Orders created successfully.");

            await _orderItemService.AddOrderItemAsync(new OrderItem
            {
                OrderId = order1.Id,
                ProductId = productEspresso.Id,
                EmployeeId = employeeAlice.Id,
                Price = productEspresso.Price,
                Count = 2,
                StartedAt = DateTime.UtcNow,
                DoneAt = DateTime.UtcNow.AddMinutes(5)
            });
            await _orderItemService.AddOrderItemAsync(new OrderItem
            {
                OrderId = order2.Id,
                ProductId = productGreenTea.Id,
                Price = productGreenTea.Price,
                Count = 1,
                StartedAt = DateTime.UtcNow,
                DoneAt = DateTime.UtcNow.AddMinutes(7)
            });
            _logger.LogInformation("Order items created successfully.");

            await _balanceHistoryService.AddHistoryAsync(new BalanceHistory
            {
                ClientId = clientJohn.Id,
                Sum = 50m,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow.AddMinutes(15),
                BalanceHistoryStatusId = statusCompleted.Id
            });
            await _balanceHistoryService.AddHistoryAsync(new BalanceHistory
            {
                ClientId = clientJane.Id,
                Sum = -20m,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow.AddMinutes(10),
                BalanceHistoryStatusId = statusPending.Id
            });
            _logger.LogInformation("Balance histories created successfully.");

            await _feedbackService.AddFeedbackAsync(new Feedback
            {
                Content = "Great service!",
                RatingId = ratingGood.Id,
                OrderId = order1.Id
            });
            await _feedbackService.AddFeedbackAsync(new Feedback
            {
                Content = "Average experience.",
                RatingId = ratingAverage.Id,
                OrderId = order2.Id
            });
            _logger.LogInformation("Feedbacks created successfully.");

            _logger.LogInformation("Displaying contents of all tables:");

            var allCategories = await _categoryService.GetAllCategoriesAsync();
            foreach (var category in allCategories)
                _logger.LogInformation("Category: {Id} - {Name}", category.Id, category.Name);

            var allEmployeeRoles = await _employeeRoleService.GetAllRolesAsync();
            foreach (var role in allEmployeeRoles)
                _logger.LogInformation("EmployeeRole: {Id} - {Name}", role.Id, role.Name);

            var allRatings = await _ratingService.GetAllRatingsAsync();
            foreach (var rating in allRatings)
                _logger.LogInformation("Rating: {Id} - {Name} ({Value})", rating.Id, rating.Name, rating.Value);

            var allBalanceHistoryStatuses = await _balanceHistoryStatusService.GetAllStatusesAsync();
            foreach (var status in allBalanceHistoryStatuses)
                _logger.LogInformation("BalanceHistoryStatus: {Id} - {Name}", status.Id, status.Name);

            var allAdmins = await _adminService.GetAllAdminsAsync();
            foreach (var a in allAdmins)
                _logger.LogInformation("Admin: {Id} - {UserName}", a.Id, a.UserName);

            var allBranches = await _branchService.GetAllBranchesAsync();
            foreach (var branch in allBranches)
                _logger.LogInformation("Branch: {Id} - {Name} ({Address})", branch.Id, branch.Name, branch.Address);

            var allClients = await _clientService.GetAllClientsAsync();
            foreach (var client in allClients)
                _logger.LogInformation("Client: {Id} - {Name} (TelegramId: {TelegramId}, Balance: {Balance})",
                    client.Id, client.Name, client.TelegramId, client.Balance);

            var allMenuPresets = await _menuPresetService.GetAllMenuPresetsAsync();
            foreach (var preset in allMenuPresets)
                _logger.LogInformation("MenuPreset: {Id} - {Name} ({Description})", preset.Id, preset.Name, preset.Description);

            var allProducts = await _productService.GetAllProductsAsync();
            foreach (var product in allProducts)
                _logger.LogInformation("Product: {Id} - {Name} ({Description}, Price: {Price}, CategoryId: {CategoryId})",
                    product.Id, product.Name, product.Description, product.Price, product.CategoryId);

            var allMenuPresetItems = await _menuPresetItemService.GetAllMenuPresetItemsAsync();
            foreach (var item in allMenuPresetItems)
                _logger.LogInformation("MenuPresetItem: {Id} - ProductId: {ProductId}, MenuPresetId: {MenuPresetId}",
                    item.Id, item.ProductId, item.MenuPresetId);

            var allEmployees = await _employeeService.GetAllEmployeesAsync();
            foreach (var emp in allEmployees)
                _logger.LogInformation("Employee: {Id} - {Name} ({UserName}, RoleId: {RoleId}, BranchId: {BranchId})",
                    emp.Id, emp.Name, emp.UserName, emp.RoleId, emp.BranchId);

            var allBranchMenus = await _branchMenuService.GetAllBranchMenusAsync();
            foreach (var bm in allBranchMenus)
                _logger.LogInformation("BranchMenu: {Id} - MenuPresetItemId: {MenuPresetItemId}, BranchId: {BranchId}, Availability: {Availability}",
                    bm.Id, bm.MenuPresetItemId, bm.BranchId, bm.Availability);

            var allOrders = await _orderService.GetAllOrdersAsync();
            foreach (var order in allOrders)
                _logger.LogInformation("Order: {Id} - ClientId: {ClientId}, BranchId: {BranchId}, CreatedAt: {CreatedAt}",
                    order.Id, order.ClientId, order.BranchId, order.CreatedAt);

            var allOrderItems = await _orderItemService.GetAllOrderItemsAsync();
            foreach (var orderItem in allOrderItems)
                _logger.LogInformation("OrderItem: {Id} - OrderId: {OrderId}, ProductId: {ProductId}, EmployeeId: {EmployeeId}, Count: {Count}",
                    orderItem.Id, orderItem.OrderId, orderItem.ProductId, orderItem.EmployeeId, orderItem.Count);

            var allBalanceHistories = await _balanceHistoryService.GetAllHistoriesAsync();
            foreach (var bh in allBalanceHistories)
                _logger.LogInformation("BalanceHistory: {Id} - ClientId: {ClientId}, Sum: {Sum}, StatusId: {StatusId}",
                    bh.Id, bh.ClientId, bh.Sum, bh.BalanceHistoryStatusId);

            var allFeedbacks = await _feedbackService.GetAllFeedbacksAsync();
            foreach (var feedback in allFeedbacks)
                _logger.LogInformation("Feedback: {Id} - {Content} (RatingId: {RatingId}, OrderId: {OrderId})",
                    feedback.Id, feedback.Content, feedback.RatingId, feedback.OrderId);

            _logger.LogInformation("Seeding process completed successfully.");
        }
    }
}
