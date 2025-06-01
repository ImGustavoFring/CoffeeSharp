using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using WebApi.Infrastructure.Extensions;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.BusinessServices.Interfaces;

namespace WebApi.Logic.BusinessServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Admin> AddAdminAsync(string userName, string password)
        {
            var existingAdmin = await _unitOfWork.Admins.GetOneAsync(x => x.UserName == userName);

            if (existingAdmin != null)
            {
                throw new InvalidOperationException("Admin with this username already exists");
            }

            var admin = new Admin
            {
                UserName = userName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _unitOfWork.Admins.AddOneAsync(admin);
            await _unitOfWork.SaveChangesAsync();

            return admin;
        }

        public async Task<(IEnumerable<Admin> Admins, int TotalCount)> GetAllAdminsAsync(
            string? userNameFilter = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<Admin, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(userNameFilter))
            {
                filter = a => a.UserName.Contains(userNameFilter);
            }

            var totalCount = await _unitOfWork.Admins.CountAsync(filter);

            var admins = await _unitOfWork.Admins.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(a => a.UserName),
                includes: null,
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (admins, totalCount);
        }

        public async Task<Admin?> GetAdminByIdAsync(long id)
        {
            return await _unitOfWork.Admins.GetByIdAsync(id);
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
            var existingAdmin = await _unitOfWork.Admins.GetByIdAsync(admin.Id);

            if (existingAdmin == null)
            {
                throw new KeyNotFoundException("Admin not found");
            }

            existingAdmin.UserName = admin.UserName;

            if (!string.IsNullOrWhiteSpace(admin.PasswordHash))
            {
                existingAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.PasswordHash);
            }

            _unitOfWork.Admins.Update(existingAdmin);
            await _unitOfWork.SaveChangesAsync();

            return existingAdmin;
        }

        public async Task DeleteAdminAsync(long id)
        {
            await _unitOfWork.Admins.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Employee> Employees, int TotalCount)> GetAllEmployeesAsync(
            string? nameFilter = null,
            string? userNameFilter = null,
            long? roleId = null,
            long? branchId = null,
            int pageIndex = 0,
            int pageSize = 50)
        {
            Expression<Func<Employee, bool>> filter = e => true;

            if (!string.IsNullOrWhiteSpace(nameFilter))
                filter = filter.AndAlso(e => e.Name.Contains(nameFilter));

            if (!string.IsNullOrWhiteSpace(userNameFilter))
                filter = filter.AndAlso(e => e.UserName.Contains(userNameFilter));

            if (roleId.HasValue)
                filter = filter.AndAlso(e => e.RoleId == roleId.Value);

            if (branchId.HasValue)
                filter = filter.AndAlso(e => e.BranchId == branchId.Value);

            var totalCount = await _unitOfWork.Employees.CountAsync(filter);

            var employees = await _unitOfWork.Employees.GetManyAsync(
                filter: filter,
                orderBy: q => q.OrderBy(e => e.Name),
                includes: new List<Expression<Func<Employee, object>>> { e => e.Role, e => e.Branch },
                disableTracking: true,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (employees, totalCount);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(long id)
        {
            return await _unitOfWork.Employees.GetByIdAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(string name,
            string userName, string password,
            long roleId, long branchId)
        {
            
            var role = await _unitOfWork.EmployeeRoles.GetByIdAsync(roleId);
            var branch = await _unitOfWork.Branches.GetByIdAsync(branchId);

            var employee = new Employee
            {
                Name = name,
                UserName = userName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RoleId = role.Id,
                BranchId = branch.Id
            };

            await _unitOfWork.Employees.AddOneAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(employee.Id);

            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.UserName = employee.UserName;

            if (!string.IsNullOrWhiteSpace(employee.PasswordHash))
            {
                existingEmployee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(employee.PasswordHash);
            }

            var role = await _unitOfWork.EmployeeRoles.GetByIdAsync(employee.RoleId);
            var branch = await _unitOfWork.Branches.GetByIdAsync(employee.BranchId);

            existingEmployee.RoleId = role.Id;
            existingEmployee.BranchId = branch.Id;

            _unitOfWork.Employees.Update(existingEmployee);
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
