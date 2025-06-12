using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class MenuPresetItem : UserControl
{
    public MenuPresetItem()
    {
        InitializeComponent();
    }

    private async void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MenuPresetItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new MenuPresetInformationWindow
            {
                DataContext = new MenuPresetInformationWindowViewModel(vm.MenuPresetDto, false)
            };
            await window.ShowDialog(ww);
        }
    }

    private async void DeletePreset(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MenuPresetItemViewModel vm)
        {
            try
            {
                await HttpClient.Instance.DeletePreset(vm.MenuPresetDto.Id);
                await DialogsHelper.ShowOk();
                MenuPresetViewModel.Instance.Presets.Remove(vm);
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }
}