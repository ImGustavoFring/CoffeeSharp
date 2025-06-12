using System;
using System.Linq;
using System.Threading.Tasks;
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
    
    [ObservableProperty]
    private bool _isCook = false;

    [ObservableProperty]
    private bool _isAuthenticated = false;

    [ObservableProperty]
    private string _authButtonText = "Войти";

    [ObservableProperty]
    private string? _userName;

    [ObservableProperty]
    private string? _userRole;

    public async Task RefreshAuthData()
    {
        var auth = AuthService.Load();
        IsAuthenticated = AuthService.Instance.IsAuthenticated;
        AuthButtonText = IsAuthenticated ? "Выйти" : "Войти";

        if (IsAuthenticated)
        {
            UserName = auth.Name ?? "Админ";
            UserRole = await GetRoleDisplayName(auth);
            IsAdminAuth = auth.UserType == "admin";
            IsHighCook = auth.UserType == "employee" && auth.RoleId == "1";
            IsCook = auth.UserType == "employee";
        }
        else
        {
            UserName = null;
            UserRole = null;
            IsAdminAuth = false;
            IsHighCook = false;
            IsCook = false;
        }
    }

    private async Task<string> GetRoleDisplayName(AuthSettings auth)
    {
        if (auth.UserType == "admin")
            return "Администратор";
        Console.WriteLine(auth.UserType);
        Console.WriteLine(auth.RoleId);
        if (auth.UserType == "employee" && !string.IsNullOrEmpty(auth.RoleId))
        {
            try
            {
                var (roles, _) = await HttpClient.Instance.GetAllEmployeeRoles();
                var role = roles.FirstOrDefault(r => r.Id.ToString() == auth.RoleId);
                return role?.Name ?? "Сотрудник";
            }
            catch
            {
                return "Сотрудник";
            }
        }
        return "";
    }

    public MainWindowViewModel()
    {
        CurrentViewModel = new LoginViewModel();
    }

    [RelayCommand]
    private void Auth()
    {
        if (IsAuthenticated)
        {
            AuthService.AccessToken = String.Empty;
            RefreshAuthData();
            NavigateToLogin();
        }
        else
        {
            NavigateToLogin();
        }
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
    private void NavigateToAdmin()
    {
        CurrentViewModel = AdminViewModel.Instance;
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
    
    [RelayCommand]
    private void NavigateToMenuPreset()
    {
        CurrentViewModel = MenuPresetViewModel.Instance;
    }
    
    [RelayCommand]
    private void NavigateToMenuPresetItems()
    {
        CurrentViewModel = MenuPresetItemsViewModel.Instance;
    }
    
    [RelayCommand]
    private void NavigateToBranchMenu()
    {
        CurrentViewModel = BranchMenuItemsViewModel.Instance;
    }
}