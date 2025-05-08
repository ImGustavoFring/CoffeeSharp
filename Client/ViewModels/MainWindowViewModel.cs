using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isMenuOpen;

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

    // All later are deprecated and will be removed
    [RelayCommand]
    private void NavigateFirst()
    {
        CurrentViewModel = new UserContol1VM();
        IsMenuOpen = false;
    }

    [RelayCommand]
    private void NavigateSecond()
    {
        CurrentViewModel = new UserControl2VM();
        IsMenuOpen = false;
    }

    [RelayCommand]
    private void ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
    }
}
