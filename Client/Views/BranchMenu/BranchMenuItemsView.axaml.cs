using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Views;

public partial class BranchMenuItemsView : UserControl
{
    public BranchMenuItemsView()
    {
        InitializeComponent();
        DataContext = BranchMenuItemsViewModel.Instance;
        SearchItems(null, null);
    }

    private async void SearchItems(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BranchMenuItemsViewModel vm)
        {
            try
            {
                long? presetId = null;
                long? branchId = null;

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

                if (!string.IsNullOrWhiteSpace(vm.BranchSearchQuery))
                {
                    var (branches, _) = await HttpClient.Instance.GetAllBranches(name: vm.BranchSearchQuery);
                    branchId = branches.FirstOrDefault()?.Id;
                }

                var items = await HttpClient.Instance.GetBranchMenus(branchId: branchId, menuPresetId: presetId);

                vm.Items.Clear();
                foreach (var item in items)
                {
                    var ivm = new BranchMenuItemsItemViewModel(new BranchMenuDtoObservable(item));
                    vm.Items.Add(ivm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreateBranchMenu(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new BranchMenuItemsItemInformationWindowViewModel(null, true);
            var window = new BranchMenuItemsItemInformationWindow
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.BranchMenuDto.Id != 0)
            {
                if (DataContext is BranchMenuItemsViewModel vm)
                {
                    vm.Items.Add(new BranchMenuItemsItemViewModel(dc.BranchMenuDto));
                }
            }
        }
    }
}