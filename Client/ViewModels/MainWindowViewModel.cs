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
        CurrentViewModel = new MainOrdersViewModel();
    }

    [RelayCommand]
    private void NavigateToBranches()
    {
        CurrentViewModel = new BranchesViewModel();
    }

    [RelayCommand]
    public void NavigateToEmployeeRoleCommand()
    {
        CurrentViewModel = new EmployeeRoleViewModel();
    }
}
