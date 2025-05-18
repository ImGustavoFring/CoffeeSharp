using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class ProductItem : UserControl
{
    public ProductItem()
    {
        InitializeComponent();
    }

    private void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ProductItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new ProductInformationWindow()
            {
                DataContext = new ProductInformationWindowViewModel(vm.ProductDto, false)
            };
            window.ShowDialog(ww);
        }
    }

    private async void DeleteProduct(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ProductItemViewModel vm)
        {
            try
            {
                await HttpClient.Instance.DeleteProduct(vm.ProductDto.Id);
                await DialogsHelper.ShowOk();
                ProductViewModel.Instance.Products.Remove(vm);
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }
}