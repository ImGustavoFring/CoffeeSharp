using System.Data;
using Dapper;
using Domain.DTOs.Log.Responses;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IDbConnection _db;

        public LogsController(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageSize = 100,
            [FromQuery] int pageIndex = 0,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] LogLevelEnum? level = null)
        {
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
                parameters.Add("Level", (int)level.Value);
                whereClauses.Add(@"""level"" = @Level");
            }

            var whereSql = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : "";

            var countSql = $@"SELECT COUNT(*) FROM ""logs"" {whereSql};";
            var totalCount = await _db.ExecuteScalarAsync<int>(countSql, parameters);

            var dataSql = $@"
                SELECT
                    ""level""      AS Level,
                    ""timestamp""  AS TimeStamp,
                    ""log_event""  AS LogEvent,
                    ""message""    AS Message,
                    ""message_template"" AS MessageTemplate,
                    ""exception""  AS Exception
                FROM ""logs""
                {whereSql}
                ORDER BY ""timestamp"" DESC
                LIMIT @Limit OFFSET @Offset;";

            parameters.Add("Limit", pageSize);
            parameters.Add("Offset", pageIndex * pageSize);

            var logs = await _db.QueryAsync<LogEntryResponse>(dataSql, parameters);

            Response.Headers.Add("X-Total-Count", totalCount.ToString());

            return Ok(logs);
        }
    }
}
