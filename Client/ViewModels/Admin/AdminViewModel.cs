using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class AdminViewModel : ViewModelBase
{
    private static readonly Lazy<AdminViewModel> _instance = new(() => new AdminViewModel());

    public static AdminViewModel Instance => _instance.Value;

    private AdminViewModel() { }

    [ObservableProperty]
    private ObservableCollection<AdminItemViewModel> _admins = new();

    [ObservableProperty]
    private string? _searchQuery = null;
}