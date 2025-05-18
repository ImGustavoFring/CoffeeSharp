using System;
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

    public MainWindowViewModel()
    {
        CurrentViewModel = new UserContol1VM();
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
    public void NavigateToEmployeeRole()
    {
        CurrentViewModel = EmployeeRoleViewModel.Instance;
    }

    [RelayCommand]
    public void NavigateToCategory()
    {
        CurrentViewModel = CategoryViewModel.Instance;
    }
}
