using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class OrderInformationWindow : Window
{
    public OrderInformationWindow()
    {
        InitializeComponent();
    }

    private async void Save(object? sender, RoutedEventArgs e)
    {
        if (DataContext is OrderInformationWindowViewModel vm)
        {
            if (await vm.Save())
            {
                await DialogsHelper.ShowOk();
                Close();
            }
            else
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private void Cancel(object? sender, RoutedEventArgs e)
    {
        if (DataContext is OrderInformationWindowViewModel vm)
        {
            vm.Cancel();
            Close();
        }
    }
}