using CoffeeSharp.Domain.Entities;
using Domain.DTOs.ReferenceData.Requests;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/references")]
    public class ReferenceDataController : ControllerBase
    {
        private readonly IReferenceDataService _referenceDataService;

        public ReferenceDataController(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        [HttpGet("ratings")]
        public async Task<IActionResult> GetAllRatings(
            [FromQuery] string? name = null,
            [FromQuery] long? value = null,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _referenceDataService.GetAllRatingsAsync(name, value, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", total.ToString());
            var dtos = items.Select(r => new RatingDto { Id = r.Id, Name = r.Name, Value = r.Value });
            return Ok(dtos);
        }

        [HttpGet("ratings/{id}")]
        public async Task<IActionResult> GetRatingById(long id)
        {
            Rating? rating = await _referenceDataService.GetRatingByIdAsync(id);
            if (rating == null)
                return NotFound();
            var dto = new RatingDto
            {
                Id = rating.Id,
                Name = rating.Name,
                Value = rating.Value
            };
            return Ok(dto);
        }

        [HttpPost("ratings")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateRating([FromBody] CreateRatingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rating = new Rating
            {
                Name = request.Name,
                Value = request.Value
            };

            Rating created = await _referenceDataService.AddRatingAsync(rating);
            var dto = new RatingDto
            {
                Id = created.Id,
                Name = created.Name,
                Value = created.Value
            };
            return CreatedAtAction(nameof(GetRatingById), new { id = dto.Id }, dto);
        }

        [HttpPut("ratings/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateRating(long id, [FromBody] UpdateRatingRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch.");

            var rating = new Rating
            {
                Id = request.Id,
                Name = request.Name,
                Value = request.Value
            };

            Rating updated = await _referenceDataService.UpdateRatingAsync(rating);
            var dto = new RatingDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Value = updated.Value
            };
            return Ok(dto);
        }

        [HttpDelete("ratings/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteRating(long id)
        {
            await _referenceDataService.DeleteRatingAsync(id);
            return NoContent();
        }

        [HttpGet("employee-roles")]
        public async Task<IActionResult> GetAllEmployeeRoles(
            [FromQuery] string? name = null,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _referenceDataService.GetAllEmployeeRolesAsync(name, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", total.ToString());
            var dtos = items.Select(r => new EmployeeRoleDto { Id = r.Id, Name = r.Name });
            return Ok(dtos);
        }

        [HttpGet("employee-roles/{id}")]
        public async Task<IActionResult> GetEmployeeRoleById(long id)
        {
            EmployeeRole? role = await _referenceDataService.GetEmployeeRoleByIdAsync(id);
            if (role == null)
                return NotFound();
            var dto = new EmployeeRoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
            return Ok(dto);
        }

        [HttpPost("employee-roles")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateEmployeeRole([FromBody] CreateEmployeeRoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = new EmployeeRole
            {
                Name = request.Name
            };

            EmployeeRole created = await _referenceDataService.AddEmployeeRoleAsync(role);
            var dto = new EmployeeRoleDto
            {
                Id = created.Id,
                Name = created.Name
            };
            return CreatedAtAction(nameof(GetEmployeeRoleById), new { id = dto.Id }, dto);
        }

        [HttpPut("employee-roles/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateEmployeeRole(long id, [FromBody] UpdateEmployeeRoleRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch.");

            var role = new EmployeeRole
            {
                Id = request.Id,
                Name = request.Name
            };

            EmployeeRole updated = await _referenceDataService.UpdateEmployeeRoleAsync(role);
            var dto = new EmployeeRoleDto
            {
                Id = updated.Id,
                Name = updated.Name
            };
            return Ok(dto);
        }

        [HttpDelete("employee-roles/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteEmployeeRole(long id)
        {
            await _referenceDataService.DeleteEmployeeRoleAsync(id);
            return NoContent();
        }

        [HttpGet("balance-history-statuses")]
        public async Task<IActionResult> GetAllBalanceHistoryStatuses(
        [FromQuery] string? name = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _referenceDataService.GetAllBalanceHistoryStatusesAsync(name, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", total.ToString());
            var dtos = items.Select(s => new BalanceHistoryStatusDto { Id = s.Id, Name = s.Name });
            return Ok(dtos);
        }

        [HttpGet("balance-history-statuses/{id}")]
        public async Task<IActionResult> GetBalanceHistoryStatusById(long id)
        {
            BalanceHistoryStatus? status = await _referenceDataService.GetBalanceHistoryStatusByIdAsync(id);
            if (status == null)
                return NotFound();
            var dto = new BalanceHistoryStatusDto
            {
                Id = status.Id,
                Name = status.Name
            };
            return Ok(dto);
        }

        [HttpPost("balance-history-statuses")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateBalanceHistoryStatus([FromBody] CreateBalanceHistoryStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var status = new BalanceHistoryStatus
            {
                Name = request.Name
            };

            BalanceHistoryStatus created = await _referenceDataService.AddBalanceHistoryStatusAsync(status);
            var dto = new BalanceHistoryStatusDto
            {
                Id = created.Id,
                Name = created.Name
            };
            return CreatedAtAction(nameof(GetBalanceHistoryStatusById), new { id = dto.Id }, dto);
        }

        [HttpPut("balance-history-statuses/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateBalanceHistoryStatus(long id, [FromBody] UpdateBalanceHistoryStatusRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch.");

            var status = new BalanceHistoryStatus
            {
                Id = request.Id,
                Name = request.Name
            };

            BalanceHistoryStatus updated = await _referenceDataService.UpdateBalanceHistoryStatusAsync(status);
            var dto = new BalanceHistoryStatusDto
            {
                Id = updated.Id,
                Name = updated.Name
            };
            return Ok(dto);
        }

        [HttpDelete("balance-history-statuses/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteBalanceHistoryStatus(long id)
        {
            await _referenceDataService.DeleteBalanceHistoryStatusAsync(id);
            return NoContent();
        }
    }
}
