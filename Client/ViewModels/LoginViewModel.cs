using System.Text;
using System.Threading.Tasks;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.ViewModels;

public partial class LoginViewModel: ViewModelBase
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(LoginButtonIsEnabled))] private string _username = string.Empty;
    
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(LoginButtonIsEnabled))] private string _password = string.Empty;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(LoginButtonIsEnabled))] private bool _inLogging = false;
    
    [ObservableProperty] private bool _loggingAsAdmin = false;
    
    [ObservableProperty] private string _errorMessage = string.Empty;

    public bool LoginButtonIsEnabled => !InLogging && Username.Length > 0 && Password.Length > 0;

    // public string ErrorMessage => new StringBuilder().Append(InLogging).Append(Username.Length)
    //     .Append(Password.Length).ToString();

    [RelayCommand]
    public async void Login()
    {
        InLogging = true;
        await Task.Delay(2000);
        ErrorMessage = string.Empty;
        try
        {
            var result = await AuthService.Instance.Login(Username, Password, LoggingAsAdmin);
            InLogging = false;
            if (result == false)
            { 
                ErrorMessage = "Неверный логин/пароль";
                return;
            }
            // todo Перекинуть на какое-нибудь окно, зависящее от роли пользователя
        }
        catch
        {
            ErrorMessage = "Неизвестная ошибка";
        }
        InLogging = false;
    }
}