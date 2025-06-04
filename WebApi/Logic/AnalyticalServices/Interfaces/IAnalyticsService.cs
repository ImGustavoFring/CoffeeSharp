using Domain.Entities.Analytics;

namespace WebApi.Logic.AnalyticalServices.Interfaces
{
    public interface IAnalyticsService
    {
        Task<(IEnumerable<SalesReportItem> Items, int TotalCount)>GetDailySalesReportAsync(
        DateTime? from,
        DateTime? to,
        int pageIndex = 0,
        int pageSize = 50);

        Task<(IEnumerable<SalesReportItem> Items, int TotalCount)>
    GetMonthlySalesReportAsync(
        DateTime? from,
        DateTime? to,
        int pageIndex = 0,
        int pageSize = 50);


        Task<(IEnumerable<EmployeePerformanceReport> Items, int TotalCount)>
    GetEmployeePerformanceAsync(
        DateTime? from,
        DateTime? to,
        int pageIndex = 0,
        int pageSize = 50);

        Task<IEnumerable<EmployeePerformanceReport>>
    GetTopRevenueEmployeesAsync(
        int top = 5,
        DateTime? from = null,
        DateTime? to = null);

        Task<IEnumerable<ProductRevenue>> GetTopRevenueProductsAsync(int top = 5);
        Task<IEnumerable<BranchRevenue>> GetTopBranchRevenueAsync(
    int top = 5,
    DateTime? from = null,
    DateTime? to = null);

        Task<IEnumerable<ClientSpending>> GetTopClientsBySpendingAsync(
    int top = 5,
    DateTime? from = null,
    DateTime? to = null);

        Task<IEnumerable<AverageOrderValueItem>> GetAverageOrderValueDailyAsync(
    DateTime? from = null,
    DateTime? to = null);

        Task<IEnumerable<BranchPendingOrders>> GetPendingOrdersByBranchAsync();

        Task<IEnumerable<CategorySales>> GetProductSalesByCategoryAsync(
    DateTime? from = null,
    DateTime? to = null);

    }
}