using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client.Views;

public partial class BranchItem : UserControl
{
    public BranchItem()
    {
        InitializeComponent();
    }

    private void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BranchItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new BranchInformationWindow()
            {
                DataContext = new BranchInformationWindowViewModel(vm.BranchDto, false)
            };
            window.ShowDialog(ww);
        }
    }
}