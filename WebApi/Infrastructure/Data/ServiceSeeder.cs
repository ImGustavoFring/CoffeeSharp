using CoffeeSharp.Domain.Entities;
using CoffeeSharp.WebApi.Infrastructure.Data;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Infrastructure.Data
{
    public static class ServiceSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, ILogger logger)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            // Получаем все сервисы
            var clientService = provider.GetRequiredService<IClientService>();
            var statusService = provider.GetRequiredService<IBalanceHistoryStatusService>();
            var balanceHistoryService = provider.GetRequiredService<IBalanceHistoryService>();
            var branchService = provider.GetRequiredService<IBranchService>();
            var branchMenuService = provider.GetRequiredService<IBranchMenuService>();
            var categoryService = provider.GetRequiredService<ICategoryService>();
            var employeeService = provider.GetRequiredService<IEmployeeService>();
            var roleService = provider.GetRequiredService<IEmployeeRoleService>();
            var feedbackService = provider.GetRequiredService<IFeedbackService>();
            var orderService = provider.GetRequiredService<IOrderService>();
            var orderItemService = provider.GetRequiredService<IOrderItemService>();
            var productService = provider.GetRequiredService<IProductService>();
            var ratingService = provider.GetRequiredService<IRatingService>();

            await SeedBalanceHistoryStatuses(statusService, logger);
            await SeedClients(clientService, logger);
            await SeedBranches(branchService, logger);
            await SeedEmployeeRoles(roleService, logger);
            await SeedCategories(categoryService, logger);
            await SeedProducts(productService, categoryService, logger);
            await SeedEmployees(employeeService, branchService, roleService, logger);
            await SeedBranchMenus(branchMenuService, branchService, productService, logger);
            await SeedRatings(ratingService, logger);
            await SeedOrders(orderService, clientService, branchService, logger);
            await SeedOrderItems(orderItemService, orderService, productService, employeeService, logger);
            await SeedFeedbacks(feedbackService, orderService, clientService, ratingService, logger);
            await SeedBalanceHistory(balanceHistoryService, clientService, statusService, logger);
        }


        private static async Task SeedBalanceHistory(
            IBalanceHistoryService balanceHistoryService,
            IClientService clientService,
            IBalanceHistoryStatusService statusService,
            ILogger logger)
        {
            if ((await balanceHistoryService.GetAllHistoriesAsync()).Any()) return;

            logger.LogInformation("Seeding Balance Histories...");

            var client = (await clientService.GetAllClientsAsync()).First();
            var status = (await statusService.GetAllStatusesAsync()).First();

            var histories = new List<BalanceHistory>
            {
                new() { ClientId = client.Id, Sum = 100m, CreatedAt = DateTime.UtcNow, BalanceHistoryStatusId = status.Id },
                new() { ClientId = client.Id, Sum = 200m, CreatedAt = DateTime.UtcNow.AddMinutes(5), BalanceHistoryStatusId = status.Id }
            };

            foreach (var history in histories)
            {
                await balanceHistoryService.AddHistoryAsync(history);
                logger.LogInformation($"Seeded BalanceHistory with Sum: {history.Sum}");
            }
        }

        private static async Task SeedBalanceHistoryStatuses(IBalanceHistoryStatusService service, ILogger logger)
        {
            if ((await service.GetAllStatusesAsync()).Any()) return;

            logger.LogInformation("Seeding Balance History Statuses...");

            var statuses = new List<BalanceHistoryStatus>
            {
                new() { Name = "Pending" },
                new() { Name = "Completed" },
                new() { Name = "Cancelled" }
            };

            foreach (var status in statuses)
            {
                await service.AddStatusAsync(status);
            }
        }

        private static async Task SeedClients(IClientService service, ILogger logger)
        {
            if ((await service.GetAllClientsAsync()).Any()) return;

            logger.LogInformation("Seeding Clients...");

            var clients = new List<Client>
            {
                new() { Name = "John Doe", Balance = 500m },
                new() { Name = "Jane Smith", Balance = 1000m }
            };

            foreach (var client in clients)
            {
                await service.AddClientAsync(client);
            }
        }

        private static async Task SeedBranches(IBranchService service, ILogger logger)
        {
            if ((await service.GetAllBranchesAsync()).Any()) return;

            logger.LogInformation("Seeding Branches...");

            var branches = new List<Branch>
            {
                new() { Name = "Central Coffee", Address = "Main Street 1" },
                new() { Name = "Downtown Coffee", Address = "Business District 5" }
            };

            foreach (var branch in branches)
            {
                await service.AddBranchAsync(branch);
            }
        }

        private static async Task SeedEmployeeRoles(IEmployeeRoleService service, ILogger logger)
        {
            if ((await service.GetAllRolesAsync()).Any()) return;

            logger.LogInformation("Seeding Employee Roles...");

            var roles = new List<EmployeeRole>
            {
                new() { Name = "Manager" },
                new() { Name = "Barista" },
                new() { Name = "Cashier" }
            };

            foreach (var role in roles)
            {
                await service.AddRoleAsync(role);
            }
        }

        private static async Task SeedCategories(ICategoryService service, ILogger logger)
        {
            if ((await service.GetAllCategoriesAsync()).Any()) return;

            logger.LogInformation("Seeding Categories...");

            var categories = new List<Category>
            {
                new() { Name = "Coffee" },
                new() { Name = "Tea" },
                new() { Name = "Desserts" }
            };

            foreach (var category in categories)
            {
                await service.AddCategoryAsync(category);
            }
        }

        private static async Task SeedProducts(
            IProductService productService,
            ICategoryService categoryService,
            ILogger logger)
        {
            if ((await productService.GetAllProductsAsync()).Any()) return;

            logger.LogInformation("Seeding Products...");

            var categories = await categoryService.GetAllCategoriesAsync();
            var coffeeCategory = categories.First(c => c.Name == "Coffee");
            var teaCategory = categories.First(c => c.Name == "Tea");
            var dessertsCategory = categories.First(c => c.Name == "Desserts");

            var products = new List<Product>
            {
                new() {
                    Name = "Espresso",
                    Price = 3.50m,
                    CategoryId = coffeeCategory.Id
                },
                new() {
                    Name = "Cappuccino",
                    Price = 4.50m,
                    CategoryId = coffeeCategory.Id
                },
                new() {
                    Name = "Green Tea",
                    Price = 2.50m,
                    CategoryId = teaCategory.Id
                },
                new() {
                    Name = "Cheesecake",
                    Price = 5.00m,
                    CategoryId = dessertsCategory.Id
                }
            };

            foreach (var product in products)
            {
                await productService.AddProductAsync(product);
            }
        }

        private static async Task SeedEmployees(
            IEmployeeService employeeService,
            IBranchService branchService,
            IEmployeeRoleService roleService,
            ILogger logger)
        {
            if ((await employeeService.GetAllEmployeesAsync()).Any()) return;

            logger.LogInformation("Seeding Employees...");

            var branches = await branchService.GetAllBranchesAsync();
            var roles = await roleService.GetAllRolesAsync();

            var employees = new List<Employee>
            {
                new() {
                    Name = "Mike Johnson",
                    BranchId = branches.First().Id,
                    RoleId = roles.First(r => r.Name == "Manager").Id
                },
                new() {
                    Name = "Sarah Wilson",
                    BranchId = branches.First().Id,
                    RoleId = roles.First(r => r.Name == "Barista").Id
                }
            };

            foreach (var employee in employees)
            {
                await employeeService.AddEmployeeAsync(employee);
            }
        }

        private static async Task SeedBranchMenus(
            IBranchMenuService branchMenuService,
            IBranchService branchService,
            IProductService productService,
            ILogger logger)
        {
            if ((await branchMenuService.GetAllBranchMenusAsync()).Any()) return;

            logger.LogInformation("Seeding Branch Menus...");

            var branches = await branchService.GetAllBranchesAsync();
            var products = await productService.GetAllProductsAsync();

            var menus = new List<BranchMenu>
            {
                new() {
                    BranchId = branches.First().Id,
                    ProductId = products.First().Id,
                    Availability = true
                },
                new() {
                    BranchId = branches.First().Id,
                    ProductId = products.Skip(1).First().Id,
                    Availability = true
                }
            };

            foreach (var menu in menus)
            {
                await branchMenuService.AddBranchMenuAsync(menu);
            }
        }

        private static async Task SeedRatings(IRatingService service, ILogger logger)
        {
            if ((await service.GetAllRatingsAsync()).Any()) return;

            logger.LogInformation("Seeding Ratings...");

            var ratings = new List<Rating>
            {
                new() { Name = "Excellent", Value = 5 },
                new() { Name = "Good", Value = 4 },
                new() { Name = "Average", Value = 3 }
            };

            foreach (var rating in ratings)
            {
                await service.AddRatingAsync(rating);
            }
        }

        private static async Task SeedOrders(
            IOrderService orderService,
            IClientService clientService,
            IBranchService branchService,
            ILogger logger)
        {
            if ((await orderService.GetAllOrdersAsync()).Any()) return;

            logger.LogInformation("Seeding Orders...");

            var clients = await clientService.GetAllClientsAsync();
            var branches = await branchService.GetAllBranchesAsync();

            var orders = new List<Order>
            {
                new() {
                    ClientId = clients.First().Id,
                    BranchId = branches.First().Id,
                    CreatedAt = DateTime.UtcNow
                }
            };

            foreach (var order in orders)
            {
                await orderService.AddOrderAsync(order);
            }
        }

        private static async Task SeedOrderItems(
            IOrderItemService orderItemService,
            IOrderService orderService,
            IProductService productService,
            IEmployeeService employeeService,
            ILogger logger)
        {
            if ((await orderItemService.GetAllOrderItemsAsync()).Any()) return;

            logger.LogInformation("Seeding Order Items...");

            var orders = await orderService.GetAllOrdersAsync();
            var products = await productService.GetAllProductsAsync();
            var employees = await employeeService.GetAllEmployeesAsync();

            var orderItems = new List<OrderItem>
            {
                new() {
                    OrderId = orders.First().Id,
                    ProductId = products.First().Id,
                    EmployeeId = employees.First().Id,
                    Price = products.First().Price,
                    Count = 2,
                    StartedAt = DateTime.UtcNow
                }
            };

            foreach (var item in orderItems)
            {
                await orderItemService.AddOrderItemAsync(item);
            }
        }

        private static async Task SeedFeedbacks(
            IFeedbackService feedbackService,
            IOrderService orderService,
            IClientService clientService,
            IRatingService ratingService,
            ILogger logger)
        {
            if ((await feedbackService.GetAllFeedbacksAsync()).Any()) return;

            logger.LogInformation("Seeding Feedbacks...");

            var orders = await orderService.GetAllOrdersAsync();
            var clients = await clientService.GetAllClientsAsync();
            var ratings = await ratingService.GetAllRatingsAsync();

            var feedbacks = new List<Feedback>
            {
                new() {
                    Content = "Great service!",
                    OrderId = orders.First().Id,
                    ClientId = clients.First().Id,
                    RatingId = ratings.First().Id
                }
            };

            foreach (var feedback in feedbacks)
            {
                await feedbackService.AddFeedbackAsync(feedback);
            }
        }
    }
}
