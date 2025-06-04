using Dapper;
using Domain.Entities.Analytics;
using WebApi.Infrastructure;
using WebApi.Logic.AnalyticalServices.Interfaces;

namespace WebApi.Logic.AnalyticalServices
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsConnectionFactory _connectionFactory;

        public AnalyticsService(IAnalyticsConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<(IEnumerable<SalesReportItem>, int)> GetDailySalesReportAsync(
            DateTime? from,
            DateTime? to,
            int pageIndex = 0,
            int pageSize = 50)
        {
            using var conn = _connectionFactory.CreateConnection();

            // Build WHERE clauses
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (from.HasValue)
            {
                whereClauses.Add("o.created_at >= @From");
                parameters.Add("From", from.Value.Date);
            }
            if (to.HasValue)
            {
                whereClauses.Add("o.created_at <= @To");
                parameters.Add("To", to.Value.Date.AddDays(1).AddTicks(-1));
            }
            var whereSql = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : "";

            // Count distinct days in range
            var countSql = $@"
                SELECT COUNT(*) 
                FROM 
                (SELECT DATE(o.created_at) AS day FROM orders o {whereSql}
                GROUP BY DATE(o.created_at)) AS daily";

            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, parameters);

            // Fetch paginated daily aggregates
            var dataSql = $@"
        SELECT 
            DATE(o.created_at) AS Date,
            SUM(oi.price * oi.count) AS TotalRevenue,
            COUNT(DISTINCT o.id)         AS OrderCount
        FROM orders o
        JOIN order_items oi ON o.id = oi.order_id
        {whereSql}
        GROUP BY DATE(o.created_at)
        ORDER BY DATE(o.created_at) DESC
        LIMIT @Limit OFFSET @Offset;";
            parameters.Add("Limit", pageSize);
            parameters.Add("Offset", pageIndex * pageSize);

            var items = await conn.QueryAsync<SalesReportItem>(dataSql, parameters);
            return (items, totalCount);
        }

        public async Task<(IEnumerable<SalesReportItem>, int)> GetMonthlySalesReportAsync(
    DateTime? from,
    DateTime? to,
    int pageIndex = 0,
    int pageSize = 50)
        {
            using var conn = _connectionFactory.CreateConnection();
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (from.HasValue)
            {
                whereClauses.Add("o.created_at >= @From");
                parameters.Add("From", from.Value.Date);
            }
            if (to.HasValue)
            {
                whereClauses.Add("o.created_at <= @To");
                parameters.Add("To", to.Value.Date.AddDays(1).AddTicks(-1));
            }
            var whereSql = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : "";

            // Count distinct months
            var countSql = $@"
        SELECT COUNT(*) 
        FROM (
            SELECT DATE_TRUNC('month', o.created_at) AS ym
            FROM orders o
            {whereSql}
            GROUP BY DATE_TRUNC('month', o.created_at)
        ) AS monthly";
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, parameters);

            // Query monthly aggregates
            var dataSql = $@"
        SELECT
            DATE_TRUNC('month', o.created_at)::DATE AS Date,
            SUM(oi.price * oi.count) AS TotalRevenue,
            COUNT(DISTINCT o.id)         AS OrderCount
        FROM orders o
        JOIN order_items oi ON o.id = oi.order_id
        {whereSql}
        GROUP BY DATE_TRUNC('month', o.created_at)
        ORDER BY DATE_TRUNC('month', o.created_at) DESC
        LIMIT @Limit OFFSET @Offset;";
            parameters.Add("Limit", pageSize);
            parameters.Add("Offset", pageIndex * pageSize);

            var items = await conn.QueryAsync<SalesReportItem>(dataSql, parameters);
            return (items, totalCount);
        }

        public async Task<(IEnumerable<EmployeePerformanceReport>, int)> GetEmployeePerformanceAsync(
    DateTime? from,
    DateTime? to,
    int pageIndex = 0,
    int pageSize = 50)
        {
            using var conn = _connectionFactory.CreateConnection();
            var whereClauses = new List<string> { "oi.employee_id IS NOT NULL", "oi.done_at IS NOT NULL" };
            var parameters = new DynamicParameters();

            if (from.HasValue)
            {
                whereClauses.Add("oi.done_at >= @From");
                parameters.Add("From", from.Value);
            }
            if (to.HasValue)
            {
                whereClauses.Add("oi.done_at <= @To");
                parameters.Add("To", to.Value);
            }
            var whereSql = "WHERE " + string.Join(" AND ", whereClauses);

            // Count total employees who have any completed items in range
            var countSql = $@"
        SELECT COUNT(DISTINCT e.id)
        FROM employees e
        JOIN order_items oi ON e.id = oi.employee_id
        {whereSql};";
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, parameters);

            // Then select paginated performance
            var dataSql = $@"
        SELECT
            e.id               AS Id,
            e.name             AS Name,
            COUNT(oi.id)       AS CompletedOrders,
            SUM(oi.price * oi.count) AS TotalRevenue
        FROM employees e
        JOIN order_items oi ON e.id = oi.employee_id
        {whereSql}
        GROUP BY e.id, e.name
        ORDER BY TotalRevenue DESC
        LIMIT @Limit OFFSET @Offset;";
            parameters.Add("Limit", pageSize);
            parameters.Add("Offset", pageIndex * pageSize);

            var items = await conn.QueryAsync<EmployeePerformanceReport>(dataSql, parameters);
            return (items, totalCount);
        }

        public async Task<IEnumerable<EmployeePerformanceReport>> GetTopRevenueEmployeesAsync(
    int top = 5,
    DateTime? from = null,
    DateTime? to = null)
        {
            using var conn = _connectionFactory.CreateConnection();
            var whereClauses = new List<string> { "oi.employee_id IS NOT NULL", "oi.done_at IS NOT NULL" };
            var parameters = new DynamicParameters();
            parameters.Add("Top", top);

            if (from.HasValue)
            {
                whereClauses.Add("oi.done_at >= @From");
                parameters.Add("From", from.Value);
            }
            if (to.HasValue)
            {
                whereClauses.Add("oi.done_at <= @To");
                parameters.Add("To", to.Value);
            }
            var whereSql = "WHERE " + string.Join(" AND ", whereClauses);

            var sql = $@"
        SELECT
            e.id               AS Id,
            e.name             AS Name,
            COUNT(oi.id)       AS CompletedOrders,
            SUM(oi.price * oi.count) AS TotalRevenue
        FROM employees e
        JOIN order_items oi ON e.id = oi.employee_id
        {whereSql}
        GROUP BY e.id, e.name
        ORDER BY TotalRevenue DESC
        LIMIT @Top;";
            return await conn.QueryAsync<EmployeePerformanceReport>(sql, parameters);
        }

        public async Task<IEnumerable<ProductRevenue>> GetTopRevenueProductsAsync(int top = 5)
        {
            using var conn = _connectionFactory.CreateConnection();
            var sql = @"
        SELECT 
            p.id, 
            p.name, 
            SUM(oi.price * oi.count) AS TotalRevenue
        FROM products p
        JOIN order_items oi ON p.id = oi.product_id
        GROUP BY p.id, p.name
        ORDER BY TotalRevenue DESC
        LIMIT @Top;";
            return await conn.QueryAsync<ProductRevenue>(sql, new { Top = top });
        }

        public async Task<IEnumerable<BranchRevenue>> GetTopBranchRevenueAsync(
    int top = 5,
    DateTime? from = null,
    DateTime? to = null)
        {
            using var conn = _connectionFactory.CreateConnection();

            // 1) Create DynamicParameters and add "Top"
            var parameters = new DynamicParameters();
            parameters.Add("Top", top);

            // 2) Build WHERE clauses based on from/to
            var whereClauses = new List<string>();
            if (from.HasValue)
            {
                whereClauses.Add("o.created_at >= @From");
                parameters.Add("From", from.Value);
            }
            if (to.HasValue)
            {
                whereClauses.Add("o.created_at <= @To");
                parameters.Add("To", to.Value);
            }
            var whereSql = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : "";

            // 3) Final SQL with pagination
            var sql = $@"
        SELECT
            b.id AS BranchId,
            b.name AS BranchName,
            SUM(oi.price * oi.count) AS TotalRevenue
        FROM branches b
        JOIN orders o ON b.id = o.branch_id
        JOIN order_items oi ON o.id = oi.order_id
        {whereSql}
        GROUP BY b.id, b.name
        ORDER BY TotalRevenue DESC
        LIMIT @Top;";

            return await conn.QueryAsync<BranchRevenue>(sql, parameters);
        }

        public async Task<IEnumerable<ClientSpending>> GetTopClientsBySpendingAsync(
    int top = 5,
    DateTime? from = null,
    DateTime? to = null)
        {
            using var conn = _connectionFactory.CreateConnection();

            // Создаём DynamicParameters и добавляем "Top"
            var parameters = new DynamicParameters();
            parameters.Add("Top", top);

            // Строим WHERE-часть
            var whereClauses = new List<string>();
            if (from.HasValue)
            {
                whereClauses.Add("o.created_at >= @From");
                parameters.Add("From", from.Value);
            }
            if (to.HasValue)
            {
                whereClauses.Add("o.created_at <= @To");
                parameters.Add("To", to.Value);
            }
            var whereSql = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : "";

            // Финальный SQL
            var sql = $@"
        SELECT
            c.id              AS ClientId,
            c.name            AS ClientName,
            SUM(oi.price * oi.count) AS TotalSpent
        FROM clients c
        JOIN orders o ON c.id = o.client_id
        JOIN order_items oi ON o.id = oi.order_id
        {whereSql}
        GROUP BY c.id, c.name
        ORDER BY TotalSpent DESC
        LIMIT @Top;";

            return await conn.QueryAsync<ClientSpending>(sql, parameters);
        }

        public async Task<IEnumerable<AverageOrderValueItem>> GetAverageOrderValueDailyAsync(
    DateTime? from = null,
    DateTime? to = null)
        {
            using var conn = _connectionFactory.CreateConnection();
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (from.HasValue)
            {
                whereClauses.Add("o.created_at >= @From");
                parameters.Add("From", from.Value.Date);
            }
            if (to.HasValue)
            {
                whereClauses.Add("o.created_at <= @To");
                parameters.Add("To", to.Value.Date.AddDays(1).AddTicks(-1));
            }
            var whereSql = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : "";

            var sql = $@"
        SELECT
            DATE(o.created_at) AS Date,
            (SUM(oi.price * oi.count)::DECIMAL / COUNT(DISTINCT o.id)) AS AverageValue
        FROM orders o
        JOIN order_items oi ON o.id = oi.order_id
        {whereSql}
        GROUP BY DATE(o.created_at)
        ORDER BY DATE(o.created_at) DESC;";
            return await conn.QueryAsync<AverageOrderValueItem>(sql, parameters);
        }

        public async Task<IEnumerable<BranchPendingOrders>> GetPendingOrdersByBranchAsync()
        {
            using var conn = _connectionFactory.CreateConnection();
            // Assuming “FinishedAt IS NULL” implies pending or in-progress
            var sql = @"
        SELECT
            b.id           AS BranchId,
            b.name         AS BranchName,
            COUNT(o.id)    AS PendingCount
        FROM branches b
        LEFT JOIN orders o ON b.id = o.branch_id
            AND o.finished_at IS NULL
        GROUP BY b.id, b.name
        ORDER BY b.name;";
            return await conn.QueryAsync<BranchPendingOrders>(sql);
        }
        public async Task<IEnumerable<CategorySales>> GetProductSalesByCategoryAsync(
    DateTime? from = null,
    DateTime? to = null)
        {
            using var conn = _connectionFactory.CreateConnection();
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (from.HasValue)
            {
                whereClauses.Add("o.created_at >= @From");
                parameters.Add("From", from.Value.Date);
            }
            if (to.HasValue)
            {
                whereClauses.Add("o.created_at <= @To");
                parameters.Add("To", to.Value.Date.AddDays(1).AddTicks(-1));
            }
            var whereSql = whereClauses.Count > 0
                ? "AND " + string.Join(" AND ", whereClauses)
                : "";

            var sql = $@"
        SELECT
            c.id                    AS CategoryId,
            c.name                  AS CategoryName,
            SUM(oi.count)           AS TotalUnitsSold,
            SUM(oi.price * oi.count) AS TotalRevenue
        FROM categories c
        JOIN products p ON c.id = p.category_id
        JOIN order_items oi ON p.id = oi.product_id
        JOIN orders o ON oi.order_id = o.id
        WHERE TRUE
        {whereSql}
        GROUP BY c.id, c.name
        ORDER BY TotalRevenue DESC;";
            return await conn.QueryAsync<CategorySales>(sql, parameters);
        }

    }
}
