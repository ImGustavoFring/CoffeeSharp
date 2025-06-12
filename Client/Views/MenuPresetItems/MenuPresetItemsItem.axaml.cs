using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class MenuPresetItemsItem : UserControl
{
    public MenuPresetItemsItem()
    {
        InitializeComponent();
    }

    private async void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MenuPresetItemsItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new MenuPresetItemsItemInformationWindow
            {
                DataContext = new MenuPresetItemsItemInformationWindowViewModel(vm.MenuPresetItemDto, false)
            };
            await window.ShowDialog(ww);
        }
    }

    private async void DeletePresetItem(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MenuPresetItemsItemViewModel vm)
        {
            try
            {
                await HttpClient.Instance.DeletePresetItem(vm.MenuPresetItemDto.Id);
                await DialogsHelper.ShowOk();
                MenuPresetItemsViewModel.Instance.Items.Remove(vm);
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }
}