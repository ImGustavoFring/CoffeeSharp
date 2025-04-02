using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;
using Domain.DTOs;
using WebApi.Logic.Features.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("admins")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllAdmins()
        {
            IEnumerable<Admin> admins = await _userService.GetAllAdminsAsync();
            IEnumerable<AdminDto> adminDtos = admins.Select(a => new AdminDto
            {
                Id = a.Id,
                UserName = a.UserName
            });
            return Ok(adminDtos);
        }

        [HttpGet("admin/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAdminById(long id)
        {
            Admin? admin = await _userService.GetAdminByIdAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            var adminDto = new AdminDto
            {
                Id = admin.Id,
                UserName = admin.UserName
            };
            return Ok(adminDto);
        }

        [HttpPost("admin/add")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddAdmin([FromBody] AdminCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Admin createdAdmin = await _userService.AddAdminAsync(request.UserName, request.Password);
            var adminDto = new AdminDto
            {
                Id = createdAdmin.Id,
                UserName = createdAdmin.UserName
            };
            return CreatedAtAction(nameof(GetAdminById), new { id = adminDto.Id }, adminDto);
        }

        [HttpPut("admin/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateAdmin(long id, [FromBody] AdminUpdateRequest request)
        {
            if (id != request.Id)
            {
                ModelState.AddModelError("Id", "URL id does not match request body id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminToUpdate = new Admin
            {
                Id = request.Id,
                UserName = request.UserName,
                PasswordHash = request.Password ?? string.Empty
            };

            Admin updatedAdmin = await _userService.UpdateAdminAsync(adminToUpdate);
            var adminDto = new AdminDto
            {
                Id = updatedAdmin.Id,
                UserName = updatedAdmin.UserName
            };
            return Ok(adminDto);
        }

        [HttpDelete("admin/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteAdmin(long id)
        {
            await _userService.DeleteAdminAsync(id);
            return NoContent();
        }
    }
}
