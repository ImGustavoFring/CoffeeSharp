using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class BranchMenuItemsItemInformationWindow : Window
{
    public BranchMenuItemsItemInformationWindow()
    {
        InitializeComponent();
    }

    private async void Save(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BranchMenuItemsItemInformationWindowViewModel vm)
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

    public void Cancel(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BranchMenuItemsItemInformationWindowViewModel vm)
        {
            vm.Cancel();
            Close();
        }
    }
}