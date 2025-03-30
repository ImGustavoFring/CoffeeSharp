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
            var admins = await _userService.GetAllAdminsAsync();
            return Ok(admins);
        }

        [HttpPost("admin/add")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddAdmin([FromBody] AdminCreateRequest request)
        {
            var createdAdmin = await _userService.AddAdminAsync(request.UserName, request.Password);
            return CreatedAtAction(nameof(GetAllAdmins), new { id = createdAdmin.Id }, new List<Admin> { createdAdmin });
        }
    }
}
