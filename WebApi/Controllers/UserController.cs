using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public UserController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("admins")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(admins);
        }

        [HttpPost("add-admin")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddAdmin([FromBody] LoginRequest admin)
        {
            var createdAdmin = await _adminService.AddAdminWithHashedPasswordAsync(new Admin() { UserName = admin.UserName, PasswordHash = admin.Password}); // need to refUCKtor
            return CreatedAtAction(nameof(GetAllAdmins), new { id = createdAdmin.Id }, createdAdmin);
        }
    }
}
