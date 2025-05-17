using Avalonia;
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
}