using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client.Views;

public partial class OrderShortItem : UserControl
{
    public OrderShortItem()
    {
        InitializeComponent();
    }

    private async void OpenOrderInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is OrderShortItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new OrderInformationWindow()
            {
                DataContext = new OrderInformationWindowViewModel(vm.OrderDtoItem)
            };
            await window.ShowDialog(ww);
        }
    }
}