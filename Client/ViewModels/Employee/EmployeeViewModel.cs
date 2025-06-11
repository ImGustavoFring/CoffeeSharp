using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class EmployeeViewModel : ViewModelBase
{
    private static readonly Lazy<EmployeeViewModel> _instance = new(() => new EmployeeViewModel());

    public static EmployeeViewModel Instance => _instance.Value;

    private EmployeeViewModel() { }

    [ObservableProperty]
    private ObservableCollection<EmployeeItemViewModel> _employees = new();

    [ObservableProperty]
    private string? _searchQuery = null;
}