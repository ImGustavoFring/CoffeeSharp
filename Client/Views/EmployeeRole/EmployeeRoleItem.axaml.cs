using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class EmployeeRoleItem : UserControl
{
    public EmployeeRoleItem()
    {
        InitializeComponent();
    }

    private void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EmployeeRoleItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new EmployeeRoleInformationWindow()
            {
                DataContext = new EmployeeRoleInformationWindowViewModel(vm.EmployeeRoleDto, false)
            };
            window.ShowDialog(ww);
        }
    }

    private async void DeleteEmployeeRole(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EmployeeRoleItemViewModel vm)
        {
            try
            {
                await HttpClient.Instance.DeleteEmployeeRole(vm.EmployeeRoleDto.Id);
                await DialogsHelper.ShowOk();
                EmployeeRoleViewModel.Instance.EmployeeRoles.Remove(vm);
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }
}