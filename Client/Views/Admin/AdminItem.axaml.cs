using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class AdminItem : UserControl
{
    public AdminItem()
    {
        InitializeComponent();
    }

    private async void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is AdminItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new AdminInformationWindow
            {
                DataContext = new AdminInformationWindowViewModel(vm.AdminDto, false)
            };
            await window.ShowDialog(ww);
        }
    }

    private async void DeleteAdmin(object? sender, RoutedEventArgs e)
    {
        if (DataContext is AdminItemViewModel vm)
        {
            try
            {
                await HttpClient.Instance.DeleteAdmin(vm.AdminDto.Id);
                await DialogsHelper.ShowOk();
                AdminViewModel.Instance.Admins.Remove(vm);
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }
}