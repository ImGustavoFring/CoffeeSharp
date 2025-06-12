using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Shared;

namespace Client.Views;

public partial class MenuPresetItemsView : UserControl
{
    public MenuPresetItemsView()
    {
        InitializeComponent();
        DataContext = MenuPresetItemsViewModel.Instance;
        SearchItems(null, null);
    }

    private async void SearchItems(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MenuPresetItemsViewModel vm)
        {
            try
            {
                long? presetId = null;
                
                if (!string.IsNullOrWhiteSpace(vm.PresetSearchQuery))
                {
                    var presets = await HttpClient.Instance.GetAllPresets(name: vm.PresetSearchQuery);
                    presetId = presets.FirstOrDefault()?.Id;
                    if (presetId == null)
                    {
                        vm.Items.Clear();
                        return;
                    }
                }
                
                var items = await HttpClient.Instance.GetPresetItems(presetId, null);
                
                if (!string.IsNullOrWhiteSpace(vm.ProductSearchQuery))
                {
                    var filteredItems = new List<MenuPresetItemDto>();
                    foreach (var item in items)
                    {
                        var observableItem = new MenuPresetItemDtoObservable(item);
                        var productName = await observableItem.GetProductNameAsync();
                        if (!string.IsNullOrEmpty(productName) &&
                            productName.Contains(vm.ProductSearchQuery, StringComparison.OrdinalIgnoreCase))
                        {
                            filteredItems.Add(item);
                        }
                    }
                    items = filteredItems;
                }

                vm.Items.Clear();
                foreach (var item in items)
                {
                    var ivm = new MenuPresetItemsItemViewModel(new MenuPresetItemDtoObservable(item));
                    vm.Items.Add(ivm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreatePresetItem(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new MenuPresetItemsItemInformationWindowViewModel(null, true);
            var window = new MenuPresetItemsItemInformationWindow
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.MenuPresetItemDto.Id != 0)
            {
                if (DataContext is MenuPresetItemsViewModel vm)
                {
                    vm.Items.Add(new MenuPresetItemsItemViewModel(dc.MenuPresetItemDto));
                }
            }
        }
    }
}