using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client.Views;

public partial class MainOrdersView : UserControl
{
    public MainOrdersView()
    {
        InitializeComponent();
        DataContext = MainOrdersViewModel.Instance;
    }

    protected void OnUnloaded()
    {
        base.OnUnloaded(null);
        if (DataContext is MainOrdersViewModel vm)
        {
            vm.Dispose();
        }
    }
}