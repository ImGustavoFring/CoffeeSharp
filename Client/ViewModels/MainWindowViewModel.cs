using System;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableObject _currentViewModel;

    [ObservableProperty]
    private bool _isAdminAuth = false;

    [ObservableProperty]
    private bool _isHighCook = false;

    public void RefreshAuthData()
    {
        var auth = AuthService.Load();
        var userType = auth.UserType;
        if (userType == "admin")
        {
            IsAdminAuth = true;
        }
        else if (userType == "employee")
        {
            IsHighCook = true;
        }
    }

    public MainWindowViewModel()
    {
        CurrentViewModel = new LoginViewModel();
    }

    [RelayCommand]
    private void NavigateToLogin()
    {
        CurrentViewModel = new LoginViewModel();
    }

    [RelayCommand]
    private void NavigateToOrders()
    {
        CurrentViewModel = MainOrdersViewModel.Instance;
    }

    [RelayCommand]
    private void NavigateToBranches()
    {
        CurrentViewModel = BranchesViewModel.Instance;
    }

    [RelayCommand]
    private void NavigateToEmployeeRole()
    {
        CurrentViewModel = EmployeeRoleViewModel.Instance;
    }
    
    [RelayCommand]
    private void NavigateToEmployee()
    {
        CurrentViewModel = EmployeeViewModel.Instance;
    }

    [RelayCommand]
    private void NavigateToCategory()
    {
        CurrentViewModel = CategoryViewModel.Instance;
    }

    [RelayCommand]
    private void NavigateToProducts()
    {
        CurrentViewModel = ProductViewModel.Instance;
    }
}
