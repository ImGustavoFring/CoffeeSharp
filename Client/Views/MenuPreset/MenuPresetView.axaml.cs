using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class MenuPresetView : UserControl
{
    public MenuPresetView()
    {
        InitializeComponent();
        DataContext = MenuPresetViewModel.Instance;
        SearchPresets(null, null);
    }

    private async void SearchPresets(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MenuPresetViewModel vm)
        {
            try
            {
                var presets = await HttpClient.Instance.GetAllPresets(vm.SearchQuery);
                vm.Presets.Clear();
                foreach (var preset in presets)
                {
                    var pvm = new MenuPresetItemViewModel(new MenuPresetDtoObservable(preset));
                    vm.Presets.Add(pvm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreatePreset(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new MenuPresetInformationWindowViewModel(null, true);
            var window = new MenuPresetInformationWindow
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.MenuPresetDto.Id != 0)
            {
                if (DataContext is MenuPresetViewModel vm)
                {
                    vm.Presets.Add(new MenuPresetItemViewModel(dc.MenuPresetDto));
                }
            }
        }
    }
}