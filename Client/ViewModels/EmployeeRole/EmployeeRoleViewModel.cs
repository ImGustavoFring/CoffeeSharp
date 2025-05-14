using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class EmployeeRoleViewModel: ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<EmployeeRoleItemViewModel> _employeeRoles = [];
    
    [ObservableProperty]
    private string? _searchNameQuery = null;
}
