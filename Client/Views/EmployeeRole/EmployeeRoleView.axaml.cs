using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class EmployeeRoleView : UserControl
{
    public EmployeeRoleView()
    {
        InitializeComponent();
        DataContext = EmployeeRoleViewModel.Instance;
        SearchEmployeeRoles(null, null);
    }
    
    private async void SearchEmployeeRoles(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EmployeeRoleViewModel vm)
        {
            var searchNameQuery = vm.SearchNameQuery;
            
            try
            {
                var rsp = await HttpClient.Instance.GetAllEmployeeRoles(searchNameQuery);
                var roles = rsp.Items;
                vm.EmployeeRoles.Clear();
                foreach (var role in roles)
                {
                    var rvm = new EmployeeRoleItemViewModel(new EmployeeRoleDtoObservable(role));         
                    vm.EmployeeRoles.Add(rvm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreateEmployeeRole(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new EmployeeRoleInformationWindowViewModel(null, true);
            var window = new EmployeeRoleInformationWindow()
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.EmployeeRoleDto.Id != 0)
            {
                if (DataContext is EmployeeRoleViewModel vm)
                {
                    vm.EmployeeRoles.Add(new EmployeeRoleItemViewModel(dc.EmployeeRoleDto));
                }
            }
        }
    }
}