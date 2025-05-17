using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class EmployeeRoleViewModel: ViewModelBase
{
    private static readonly Lazy<EmployeeRoleViewModel> _instance = new(() => new EmployeeRoleViewModel());

    public static EmployeeRoleViewModel Instance => _instance.Value;

    private EmployeeRoleViewModel() { }
    
    [ObservableProperty]
    private ObservableCollection<EmployeeRoleItemViewModel> _employeeRoles = [];
    
    [ObservableProperty]
    private string? _searchNameQuery = null;
}
