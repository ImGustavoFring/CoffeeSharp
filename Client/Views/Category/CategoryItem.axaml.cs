using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class CategoryItem : UserControl
{
    public CategoryItem()
    {
        InitializeComponent();
        if (DataContext is CategoryItemViewModel vm)
        {
            vm.GetParentCategory().GetAwaiter().GetResult();
        }
    }

    private async void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CategoryItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new CategoryInformationWindow()
            {
                DataContext = new CategoryInformationWindowViewModel(vm.CategoryDto, false)
            };
            await window.ShowDialog(ww);
        }
    }

    private async void DeleteCategory(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CategoryItemViewModel vm)
        {
            try
            {
                await HttpClient.Instance.DeleteCategory(vm.CategoryDto.Id);
                await DialogsHelper.ShowOk();
                CategoryViewModel.Instance.Categories.Remove(vm);
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }
}