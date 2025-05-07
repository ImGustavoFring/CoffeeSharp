using Domain.DTOs.Auth.Requests;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.AdminLoginAsync(
                request.UserName, request.Password);

            return Ok(new { token });
        }

        [HttpPost("employee/login")]
        public async Task<IActionResult> EmployeeLogin([FromBody] EmployeeLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.EmployeeLoginAsync(
                request.UserName, request.Password);

            return Ok(new { token });
        }
    }
}
