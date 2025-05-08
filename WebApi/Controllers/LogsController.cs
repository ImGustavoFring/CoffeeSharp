using System.Data;
using Dapper;
using Domain.DTOs.Log.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            [FromQuery] int pageIndex = 0)
        {
            const string sql = @"
            SELECT 
                ""level"",
                ""timestamp"",
                ""log_event"",
                ""message"",
                ""message_template"",
                ""exception""
            FROM ""logs""
            ORDER BY ""timestamp"" DESC
            LIMIT @Limit OFFSET @Offset";

            var logs = await _db.QueryAsync<LogEntryResponse>(sql, new { Limit = pageSize, Offset = pageIndex });
            return Ok(logs);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetLogsCount()
        {
            const string sql = "SELECT COUNT(*) FROM \"logs\"";

            var count = await _db.ExecuteScalarAsync<int>(sql);

            return Ok(count);
        }
    }
}