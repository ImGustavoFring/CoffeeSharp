using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class ProductView : UserControl
{
    public ProductView()
    {
        InitializeComponent();
        DataContext = ProductViewModel.Instance;
        SearchProducts(null, null);
    }
    
    private async void SearchProducts(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ProductViewModel vm)
        {
            var searchNameQuery = vm.SearchNameQuery;
            
            try
            {
                var rsp = await HttpClient.Instance.GetAllProducts(searchNameQuery);
                var products = rsp.Items;
                vm.Products.Clear();
                foreach (var product in products)
                {
                    var pvm = new ProductItemViewModel(new ProductDtoObservable(product));         
                    vm.Products.Add(pvm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreateProduct(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new ProductInformationWindowViewModel(null, true);
            var window = new ProductInformationWindow()
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.ProductDto.Id != 0)
            {
                if (DataContext is ProductViewModel vm)
                {
                    vm.Products.Add(new ProductItemViewModel(dc.ProductDto));
                }
            }
        }
    }
}