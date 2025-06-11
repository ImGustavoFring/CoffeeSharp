using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class EmployeeItem : UserControl
{
    public EmployeeItem()
    {
        InitializeComponent();
    }

    private async void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EmployeeItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new EmployeeInformationWindow
            {
                DataContext = new EmployeeInformationWindowViewModel(vm.EmployeeDto, false)
            };
            await window.ShowDialog(ww);
        }
    }

    private async void DeleteEmployee(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EmployeeItemViewModel vm)
        {
            try
            {
                await HttpClient.Instance.DeleteEmployee(vm.EmployeeDto.Id);
                await DialogsHelper.ShowOk();
                EmployeeViewModel.Instance.Employees.Remove(vm);
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }
}