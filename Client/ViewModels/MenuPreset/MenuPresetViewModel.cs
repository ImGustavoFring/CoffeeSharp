using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class MenuPresetViewModel : ViewModelBase
{
    private static readonly Lazy<MenuPresetViewModel> _instance = new(() => new MenuPresetViewModel());

    public static MenuPresetViewModel Instance => _instance.Value;

    private MenuPresetViewModel() { }

    [ObservableProperty]
    private ObservableCollection<MenuPresetItemViewModel> _presets = new();

    [ObservableProperty]
    private string? _searchQuery = null;
}