using CoffeeSharp.Domain.Entities;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/branches")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranches(
            [FromQuery] string? name,
            [FromQuery] string? address,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 50)
        {
            var (branches, total) = await _branchService.GetBranchesAsync(name, address, page, pageSize);

            Response.Headers.Add("X-Total-Count", total.ToString());

            var branchDtos = branches.Select(branch => new BranchDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address
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
        public async Task<IActionResult> GetBranchMenus(
         [FromQuery] long? branchId,
         [FromQuery] long? menuPresetItemId,
         [FromQuery] long? menuPresetId,
         [FromQuery] bool? availability,
         [FromQuery] int page = 0,
         [FromQuery] int pageSize = 50)
        {
            var branchMenus = await _branchService.GetAllBranchMenusAsync(
                branchId,
                menuPresetItemId,
                menuPresetId,
                availability,
                page,
                pageSize);

            var dto = branchMenus.Select(branchMenu => new BranchMenuDto
            {
                Id = branchMenu.Id,
                BranchId = branchMenu.BranchId,
                MenuPresetItemId = branchMenu.MenuPresetItemsId,
                Availability = branchMenu.Availability
            });

            return Ok(dto);
        }



        [HttpPatch("menus/{id}/availability")]
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
