using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs.Log.Responses;
using Domain.Enums;
using WebApi.Logic.BusinessServices.Interfaces;

namespace WebApi.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsService _logsService;

        public LogsController(ILogsService logsService)
        {
            _logsService = logsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageSize = 100,
            [FromQuery] int pageIndex = 0,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] LogLevelEnum? level = null)
        {
            var (logs, totalCount) = await _logsService.GetLogsAsync(pageSize, pageIndex, from, to, level);

            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            return Ok(logs);
        }
    }
}
