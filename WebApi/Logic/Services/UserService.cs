using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Admin> AddAdminAsync(string userName, string password)
        {
            var existingAdmin = await _unitOfWork.Admins.GetSingleAsync(x => x.UserName == userName);
            if (existingAdmin != null)
            {
                throw new InvalidOperationException("Admin with this username already exists");
            }

            var admin = new Admin
            {
                UserName = userName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _unitOfWork.Admins.AddAsync(admin);
            await _unitOfWork.SaveChangesAsync();
            return admin;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _unitOfWork.Admins.GetAllAsync();
        }

        public async Task<Admin?> GetAdminByIdAsync(long id)
        {
            return await _unitOfWork.Admins.GetByIdAsync(id);
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
            var existingAdmin = await _unitOfWork.Admins.GetByIdAsync(admin.Id);
            if (existingAdmin == null)
                throw new KeyNotFoundException("Admin not found");

            existingAdmin.UserName = admin.UserName;

            if (!string.IsNullOrWhiteSpace(admin.PasswordHash))
            {
                existingAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.PasswordHash);
            }

            await _unitOfWork.Admins.UpdateAsync(existingAdmin);
            await _unitOfWork.SaveChangesAsync();

            return existingAdmin;
        }

        public async Task DeleteAdminAsync(long id)
        {
            await _unitOfWork.Admins.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _unitOfWork.Employees.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(long id)
        {
            return await _unitOfWork.Employees.GetByIdAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(string name, string userName, string password, long roleId, long branchId)
        {
            var employee = new Employee
            {
                Name = name,
                UserName = userName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RoleId = roleId,
                BranchId = branchId
            };

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(employee.Id);
            if (existingEmployee == null)
                throw new KeyNotFoundException("Employee not found");

            existingEmployee.Name = employee.Name;
            existingEmployee.UserName = employee.UserName;
            if (!string.IsNullOrWhiteSpace(employee.PasswordHash))
            {
                existingEmployee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(employee.PasswordHash);
            }
            existingEmployee.RoleId = employee.RoleId;
            existingEmployee.BranchId = employee.BranchId;

            await _unitOfWork.Employees.UpdateAsync(existingEmployee);
            await _unitOfWork.SaveChangesAsync();

            return existingEmployee;
        }

        public async Task DeleteEmployeeAsync(long id)
        {
            await _unitOfWork.Employees.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
