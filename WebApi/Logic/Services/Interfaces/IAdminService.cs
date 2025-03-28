﻿using Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
        Task<Admin?> GetAdminByIdAsync(int id);
        Task<Admin> AddAdminAsync(Admin admin);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(int id);
    }
}
