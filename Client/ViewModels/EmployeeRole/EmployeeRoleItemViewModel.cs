using System;
using Client.ObservableDTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class EmployeeRoleItemViewModel(EmployeeRoleDtoObservable employeeRoleDto) : ViewModelBase
{
    [ObservableProperty] private EmployeeRoleDtoObservable _employeeRoleDto = employeeRoleDto;
    
}
