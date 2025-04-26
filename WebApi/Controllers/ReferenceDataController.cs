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
    [Route("api/reference")]
    public class ReferenceDataController : ControllerBase
    {
        private readonly IReferenceDataService _referenceDataService;

        public ReferenceDataController(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        [HttpGet("ratings")]
        public async Task<IActionResult> GetAllRatings()
        {
            IEnumerable<Rating> ratings = await _referenceDataService.GetAllRatingsAsync();
            IEnumerable<RatingDto> ratingDtos = ratings.Select(r => new RatingDto
            {
                Id = r.Id,
                Name = r.Name,
                Value = r.Value
            });
            return Ok(ratingDtos);
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

        [HttpGet("employeeRoles")]
        public async Task<IActionResult> GetAllEmployeeRoles()
        {
            IEnumerable<EmployeeRole> roles = await _referenceDataService.GetAllEmployeeRolesAsync();
            IEnumerable<EmployeeRoleDto> roleDtos = roles.Select(r => new EmployeeRoleDto
            {
                Id = r.Id,
                Name = r.Name
            });
            return Ok(roleDtos);
        }

        [HttpGet("employeeRoles/{id}")]
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

        [HttpPost("employeeRoles")]
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

        [HttpPut("employeeRoles/{id}")]
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

        [HttpDelete("employeeRoles/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteEmployeeRole(long id)
        {
            await _referenceDataService.DeleteEmployeeRoleAsync(id);
            return NoContent();
        }

        [HttpGet("balanceHistoryStatuses")]
        public async Task<IActionResult> GetAllBalanceHistoryStatuses()
        {
            IEnumerable<BalanceHistoryStatus> statuses = await _referenceDataService.GetAllBalanceHistoryStatusesAsync();
            IEnumerable<BalanceHistoryStatusDto> statusDtos = statuses.Select(s => new BalanceHistoryStatusDto
            {
                Id = s.Id,
                Name = s.Name
            });
            return Ok(statusDtos);
        }

        [HttpGet("balanceHistoryStatuses/{id}")]
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

        [HttpPost("balanceHistoryStatuses")]
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

        [HttpPut("balanceHistoryStatuses/{id}")]
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

        [HttpDelete("balanceHistoryStatuses/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteBalanceHistoryStatus(long id)
        {
            await _referenceDataService.DeleteBalanceHistoryStatusAsync(id);
            return NoContent();
        }
    }
}
