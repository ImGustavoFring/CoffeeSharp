using CoffeeSharp.Domain.Entities;
using CoffeeSharp.WebApi.Infrastructure.Data;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Infrastructure.Data
{
    public static class ServiceSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, ILogger logger)
        {
            var clientService = services.GetRequiredService<IClientService>();
            var statusService = services.GetRequiredService<IBalanceHistoryStatusService>();
            var balanceHistoryService = services.GetRequiredService<IBalanceHistoryService>();

            // Seed BalanceHistoryStatuses
            var statuses = await statusService.GetAllStatusesAsync();
            if (!statuses.Any())
            {
                logger.LogInformation("Seeding BalanceHistoryStatuses...");

                var pendingStatus = new BalanceHistoryStatus { Name = "Pending" };
                var completedStatus = new BalanceHistoryStatus { Name = "Completed" };

                await statusService.AddStatusAsync(pendingStatus);
                await statusService.AddStatusAsync(completedStatus);

                logger.LogInformation("BalanceHistoryStatuses seeded successfully.");

                // Вывод добавленных статусов
                statuses = await statusService.GetAllStatusesAsync();
                foreach (var status in statuses)
                {
                    logger.LogInformation($"Status: Id = {status.Id}, Name = {status.Name}");
                }
            }

            // Seed Clients
            var clients = await clientService.GetAllClientsAsync();
            if (!clients.Any())
            {
                logger.LogInformation("Seeding Clients...");

                var client1 = new Client { Name = "Alice", Balance = 100m };
                var client2 = new Client { Name = "Bob", Balance = 200m };

                await clientService.AddClientAsync(client1);
                await clientService.AddClientAsync(client2);

                logger.LogInformation("Clients seeded successfully.");

                // Вывод добавленных клиентов
                clients = await clientService.GetAllClientsAsync();
                foreach (var client in clients)
                {
                    logger.LogInformation($"Client: Id = {client.Id}, Name = {client.Name}, Balance = {client.Balance}");
                }
            }

            // Seed BalanceHistories
            var firstClient = (await clientService.GetAllClientsAsync()).FirstOrDefault();
            if (firstClient != null)
            {
                var histories = await balanceHistoryService.GetAllHistoriesAsync();
                if (!histories.Any())
                {
                    logger.LogInformation("Seeding BalanceHistories...");

                    statuses = await statusService.GetAllStatusesAsync();
                    var pendingStatus = statuses.FirstOrDefault(s => s.Name == "Pending");

                    if (pendingStatus != null)
                    {
                        var balanceHistory = new BalanceHistory
                        {
                            ClientId = firstClient.Id,
                            Sum = 50m,
                            CreatedAt = DateTime.UtcNow,
                            FinishedAt = null,
                            BalanceHistoryStatusId = pendingStatus.Id
                        };

                        await balanceHistoryService.AddHistoryAsync(balanceHistory);
                        logger.LogInformation("BalanceHistories seeded successfully.");

                        // Вывод добавленных историй баланса
                        histories = await balanceHistoryService.GetAllHistoriesAsync();
                        foreach (var history in histories)
                        {
                            logger.LogInformation($"BalanceHistory: Id = {history.Id}, ClientId = {history.ClientId}, Sum = {history.Sum}, CreatedAt = {history.CreatedAt}, FinishedAt = {history.FinishedAt}, StatusId = {history.BalanceHistoryStatusId}");
                        }
                    }
                }
            }
        }
    }
}
