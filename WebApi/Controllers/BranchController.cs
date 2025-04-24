using CoffeeSharp.Domain.Entities;
using Domain.DTOs;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.Branch.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/branch")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranches([FromQuery] GetBranchesRequest request)
        {
            IEnumerable<Branch> branches = await _branchService.GetBranchesAsync(
                request.SearchTerm, request.Page, request.PageSize);

            IEnumerable<BranchDto> branchDtos = branches.Select(b => new BranchDto
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address
            });

            return Ok(branchDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchById(long id)
        {
            Branch? branch = await _branchService.GetBranchByIdAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            var branchDto = new BranchDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address
            };
            return Ok(branchDto);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var branch = new Branch
            {
                Name = request.Name,
                Address = request.Address
            };

            Branch created = await _branchService.AddBranchAsync(branch);
            var branchDto = new BranchDto
            {
                Id = created.Id,
                Name = created.Name,
                Address = created.Address
            };
            return CreatedAtAction(nameof(GetBranchById), new { id = branchDto.Id }, branchDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateBranch(long id, [FromBody] UpdateBranchRequest request)
        {
            if (id != request.Id)
            {
                ModelState.AddModelError("Id", "URL id does not match request body id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var branch = new Branch
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address
            };

            Branch updated = await _branchService.UpdateBranchAsync(branch);
            var branchDto = new BranchDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Address = updated.Address
            };
            return Ok(branchDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteBranch(long id)
        {
            await _branchService.DeleteBranchAsync(id);
            return NoContent();
        }

        [HttpPost("{branchId}/menupreset")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AssignMenuPresetToBranch(long branchId, [FromBody] AssignMenuPresetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _branchService.AssignMenuPresetToBranchAsync(branchId, request.MenuPresetId);
            return NoContent();
        }

        [HttpGet("menus")]
        public async Task<IActionResult> GetBranchMenus([FromQuery] GetBranchMenusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menus = await _branchService.GetAllBranchMenusAsync(
                request.BranchId,
                request.MenuPresetItemsId,
                request.Availability,
                request.Page,
                request.PageSize);

            var dto = menus.Select(m => new BranchMenuDto
            {
                Id = m.Id,
                BranchId = m.BranchId,
                MenuPresetItemId = m.MenuPresetItemsId,
                Availability = m.Availability
            });

            return Ok(dto);
        }

        [HttpPatch("menu/{id}/availability")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateBranchMenuAvailability(long id, [FromQuery] bool availability)
        {
            BranchMenu updated = await _branchService.UpdateBranchMenuAvailabilityAsync(id, availability);
            var menuDto = new BranchMenuDto
            {
                Id = updated.Id,
                MenuPresetItemId = updated.MenuPresetItemsId,
                BranchId = updated.BranchId,
                Availability = updated.Availability
            };
            return Ok(menuDto);
        }
    }
}
