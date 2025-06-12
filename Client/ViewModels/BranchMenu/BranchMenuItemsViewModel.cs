using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class BranchMenuItemsViewModel : ViewModelBase
{
    private static readonly Lazy<BranchMenuItemsViewModel> _instance = new(() => new BranchMenuItemsViewModel());

    public static BranchMenuItemsViewModel Instance => _instance.Value;

    private BranchMenuItemsViewModel() { }

    [ObservableProperty]
    private ObservableCollection<BranchMenuItemsItemViewModel> _items = new();

    [ObservableProperty]
    private string? _presetSearchQuery = null;

    [ObservableProperty]
    private string? _branchSearchQuery = null;
}