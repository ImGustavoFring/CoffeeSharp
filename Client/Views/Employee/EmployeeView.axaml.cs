using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class EmployeeView : UserControl
{
    public EmployeeView()
    {
        InitializeComponent();
        DataContext = EmployeeViewModel.Instance;
        SearchEmployees(null, null);
    }

    private async void SearchEmployees(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EmployeeViewModel vm)
        {
            try
            {
                var rsp = await HttpClient.Instance.GetAllEmployees(name: vm.SearchQuery);
                var employees = rsp.Employees;
                vm.Employees.Clear();
                foreach (var employee in employees)
                {
                    var evm = new EmployeeItemViewModel(new EmployeeDtoObservable(employee));
                    vm.Employees.Add(evm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreateEmployee(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new EmployeeInformationWindowViewModel(null, true);
            var window = new EmployeeInformationWindow
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.EmployeeDto.Id != 0)
            {
                if (DataContext is EmployeeViewModel vm)
                {
                    vm.Employees.Add(new EmployeeItemViewModel(dc.EmployeeDto));
                }
            }
        }
    }
}