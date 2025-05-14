using System;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.ReferenceData.Requests;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class EmployeeRoleInformationWindowViewModel(EmployeeRoleDtoObservable? employeeRoleDto, bool isNew) : ViewModelBase
{
    [ObservableProperty] private EmployeeRoleDtoObservable _employeeRoleDto = employeeRoleDto ?? new EmployeeRoleDtoObservable();
    [ObservableProperty] private bool _isNew = isNew;

    private readonly EmployeeRoleDto _backupEmployeeRoleDto = new EmployeeRoleDto()
    {
        Name = employeeRoleDto != null ? employeeRoleDto.Name: string.Empty
    };

    public async Task<bool> Save()
    {
        try
        {
            if (IsNew)
            {
                var employeeRoleDto = await HttpClient.Instance.CreateEmployeeRole(new CreateEmployeeRoleRequest()
                {
                    Name = EmployeeRoleDto.Name
                });
                EmployeeRoleDto.Id = employeeRoleDto.Id;
                EmployeeRoleDto.Name = employeeRoleDto.Name;
            }
            else
            {
                await HttpClient.Instance.UpdateEmployeeRole(EmployeeRoleDto.Id, new UpdateEmployeeRoleRequest()
                {
                    Id = EmployeeRoleDto.Id,
                    Name = EmployeeRoleDto.Name
                });
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Cancel()
    {
        EmployeeRoleDto.Name = _backupEmployeeRoleDto.Name;
    }
}