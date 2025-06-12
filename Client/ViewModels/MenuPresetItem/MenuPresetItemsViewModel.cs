using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class MenuPresetItemsViewModel : ViewModelBase
{
    private static readonly Lazy<MenuPresetItemsViewModel> _instance = new(() => new MenuPresetItemsViewModel());

    public static MenuPresetItemsViewModel Instance => _instance.Value;

    private MenuPresetItemsViewModel() { }

    [ObservableProperty]
    private ObservableCollection<MenuPresetItemsItemViewModel> _items = new();

    [ObservableProperty]
    private string? _presetSearchQuery = null;

    [ObservableProperty]
    private string? _productSearchQuery = null;
}