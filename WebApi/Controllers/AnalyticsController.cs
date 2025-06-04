using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.AnalyticalServices.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("sales/daily")]
        public async Task<IActionResult> GetDailySalesReport(
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _analyticsService
                .GetDailySalesReportAsync(from, to, pageIndex, pageSize);

            Response.Headers.Add("X-Total-Count", total.ToString());
            return Ok(items);
        }

        [HttpGet("sales/monthly")]
        public async Task<IActionResult> GetMonthlySalesReport(
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null,
    [FromQuery] int pageIndex = 0,
    [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _analyticsService
                .GetMonthlySalesReportAsync(from, to, pageIndex, pageSize);

            Response.Headers.Add("X-Total-Count", total.ToString());
            return Ok(items);
        }

        [HttpGet("employees/performance")]
        public async Task<IActionResult> GetEmployeePerformance(
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null,
    [FromQuery] int pageIndex = 0,
    [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _analyticsService
                .GetEmployeePerformanceAsync(from, to, pageIndex, pageSize);

            Response.Headers.Add("X-Total-Count", total.ToString());
            return Ok(items);
        }

        [HttpGet("employees/top-revenue")]
        public async Task<IActionResult> GetTopRevenueEmployees(
    [FromQuery] int top = 5,
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var items = await _analyticsService
                .GetTopRevenueEmployeesAsync(top, from, to);
            return Ok(items);
        }

        [HttpGet("products/top-revenue")]
        public async Task<IActionResult> GetTopRevenueProducts(
    [FromQuery] int top = 5)
        {
            var items = await _analyticsService.GetTopRevenueProductsAsync(top);
            return Ok(items);
        }

        [HttpGet("branches/top-revenue")]
        public async Task<IActionResult> GetTopBranchesByRevenue(
    [FromQuery] int top = 5,
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var items = await _analyticsService
                .GetTopBranchRevenueAsync(top, from, to);
            return Ok(items);
        }


        [HttpGet("clients/top-spending")]
        public async Task<IActionResult> GetTopClientsBySpending(
    [FromQuery] int top = 5,
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var items = await _analyticsService
                .GetTopClientsBySpendingAsync(top, from, to);
            return Ok(items);
        }

        [HttpGet("orders/average-daily")]
        public async Task<IActionResult> GetAverageOrderValueDaily(
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var items = await _analyticsService
                .GetAverageOrderValueDailyAsync(from, to);
            return Ok(items);
        }

        [HttpGet("branches/pending-orders")]
        public async Task<IActionResult> GetPendingOrdersByBranch()
        {
            var items = await _analyticsService
                .GetPendingOrdersByBranchAsync();
            return Ok(items);
        }

        [HttpGet("categories/sales")]
        public async Task<IActionResult> GetProductSalesByCategory(
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var items = await _analyticsService
                .GetProductSalesByCategoryAsync(from, to);
            return Ok(items);
        }


    }
}
