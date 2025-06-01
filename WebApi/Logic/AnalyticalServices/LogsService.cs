using System.Data;
using Dapper;
using Domain.DTOs.Log.Responses;
using Domain.Enums;
using WebApi.Logic.BusinessServices.Interfaces;

namespace WebApi.Logic.AnalyticalServices
{
    public class LogsService : ILogsService
    {
        private readonly IDbConnection _db;

        public LogsService(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<(IEnumerable<LogEntryResponse> Logs, int TotalCount)> GetLogsAsync(
            int pageSize,
            int pageIndex,
            DateTime? from,
            DateTime? to,
            LogLevelEnum? level)
        {
            // Формируем WHERE-часть и параметры
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (from.HasValue)
            {
                whereClauses.Add(@"""timestamp"" >= @From");
                parameters.Add("From", from.Value);
            }

            if (to.HasValue)
            {
                whereClauses.Add(@"""timestamp"" <= @To");
                parameters.Add("To", to.Value);
            }

            if (level.HasValue)
            {
                whereClauses.Add(@"""level"" = @Level");
                parameters.Add("Level", (int)level.Value);
            }

            var whereSql = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : string.Empty;

            // Сначала – подсчёт общего числа строк
            var countSql = $@"SELECT COUNT(*) FROM ""logs"" {whereSql};";
            var totalCount = await _db.ExecuteScalarAsync<int>(countSql, parameters);

            // Затем – выборка постранично
            var dataSql = $@"
                SELECT
                    ""level""            AS Level,
                    ""timestamp""        AS TimeStamp,
                    ""log_event""        AS LogEvent,
                    ""message""          AS Message,
                    ""message_template"" AS MessageTemplate,
                    ""exception""        AS Exception
                FROM ""logs""
                {whereSql}
                ORDER BY ""timestamp"" DESC
                LIMIT @Limit OFFSET @Offset;";

            parameters.Add("Limit", pageSize);
            parameters.Add("Offset", pageIndex * pageSize);

            var logs = await _db.QueryAsync<LogEntryResponse>(dataSql, parameters);

            return (logs, totalCount);
        }
    }
}
