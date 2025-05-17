using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class BranchesViewModel: ViewModelBase
{
    private static readonly Lazy<BranchesViewModel> _instance = new(() => new BranchesViewModel());

    public static BranchesViewModel Instance => _instance.Value;

    private BranchesViewModel() { }
    
    [ObservableProperty]
    private ObservableCollection<BranchItemViewModel> _branches = [];

    [ObservableProperty]
    private string? _searchNameQuery = null;
    
    [ObservableProperty]
    private string? _searchAddressQuery = null;
    
}
