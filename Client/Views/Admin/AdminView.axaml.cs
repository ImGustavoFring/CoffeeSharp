using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class AdminView : UserControl
{
    public AdminView()
    {
        InitializeComponent();
        DataContext = AdminViewModel.Instance;
        SearchAdmins(null, null);
    }

    private async void SearchAdmins(object? sender, RoutedEventArgs e)
    {
        if (DataContext is AdminViewModel vm)
        {
            try
            {
                var rsp = await HttpClient.Instance.GetAllAdmins(vm.SearchQuery);
                var admins = rsp.Admins;
                vm.Admins.Clear();
                foreach (var admin in admins)
                {
                    var avm = new AdminItemViewModel(new AdminDtoObservable(admin));
                    vm.Admins.Add(avm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreateAdmin(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new AdminInformationWindowViewModel(null, true);
            var window = new AdminInformationWindow
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.AdminDto.Id != 0)
            {
                if (DataContext is AdminViewModel vm)
                {
                    vm.Admins.Add(new AdminItemViewModel(dc.AdminDto));
                }
            }
        }
    }
}