using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;
using Domain.DTOs;
using CoffeeSharp.Domain.Entities;

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
        public async Task<IActionResult> AddAdmin([FromBody] CreateAdminRequest request)
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
        public async Task<IActionResult> UpdateAdmin(long id, [FromBody] UpdateAdminRequest request)
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

        [HttpGet("employees")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> GetAllEmployees()
        {
            IEnumerable<Employee> employees = await _userService.GetAllEmployeesAsync();
            IEnumerable<EmployeeDto> employeeDtos = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                UserName = e.UserName,
                RoleId = e.RoleId,
                BranchId = e.BranchId
            });
            return Ok(employeeDtos);
        }

        [HttpGet("employee/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetEmployeeById(long id)
        {
            Employee? employee = await _userService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var dto = new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                UserName = employee.UserName,
                RoleId = employee.RoleId,
                BranchId = employee.BranchId
            };
            return Ok(dto);
        }

        [HttpPost("employee")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Employee employee = await _userService.AddEmployeeAsync(request.Name, request.UserName, request.Password, request.RoleId, request.BranchId);
            var dto = new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                UserName = employee.UserName,
                RoleId = employee.RoleId,
                BranchId = employee.BranchId
            };
            return CreatedAtAction(nameof(GetEmployeeById), new { id = dto.Id }, dto);
        }

        [HttpPut("employee/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateEmployee(long id, [FromBody] UpdateEmployeeRequest request)
        {
            if (id != request.Id)
            {
                ModelState.AddModelError("Id", "URL id does not match request body id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Employee
            {
                Id = request.Id,
                Name = request.Name,
                UserName = request.UserName,
                PasswordHash = request.Password ?? string.Empty,
                RoleId = request.RoleId,
                BranchId = request.BranchId
            };

            Employee updated = await _userService.UpdateEmployeeAsync(employee);
            var dto = new EmployeeDto
            {
                Id = updated.Id,
                Name = updated.Name,
                UserName = updated.UserName,
                RoleId = updated.RoleId,
                BranchId = updated.BranchId
            };
            return Ok(dto);
        }

        [HttpDelete("employee/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            await _userService.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}
