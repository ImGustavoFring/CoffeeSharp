using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class BranchesViewModel: ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BranchItemViewModel> _branches = [];

    [ObservableProperty]
    private string? _searchNameQuery = null;
    
    [ObservableProperty]
    private string? _searchAddressQuery = null;

    public BranchesViewModel()
    {
        
    }
}