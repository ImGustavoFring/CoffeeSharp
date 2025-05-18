using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class CategoryView : UserControl
{
    public CategoryView()
    {
        InitializeComponent();
        DataContext = CategoryViewModel.Instance;
        SearchCategories(null, null);
    }
    
    private async void SearchCategories(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CategoryViewModel vm)
        {
            var searchNameQuery = vm.SearchNameQuery;
            
            try
            {
                var rsp = await HttpClient.Instance.GetAllCategories(searchNameQuery);
                var categories = rsp.Items;
                vm.Categories.Clear();
                foreach (var category in categories)
                {
                    var cvm = new CategoryItemViewModel(new CategoryDtoObservable(category));         
                    vm.Categories.Add(cvm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreateCategory(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new CategoryInformationWindowViewModel(null, true);
            var window = new CategoryInformationWindow()
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.CategoryDto.Id != 0)
            {
                if (DataContext is CategoryViewModel vm)
                {
                    vm.Categories.Add(new CategoryItemViewModel(dc.CategoryDto));
                }
            }
        }
    }
}