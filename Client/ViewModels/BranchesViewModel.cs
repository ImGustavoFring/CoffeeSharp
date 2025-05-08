using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class BranchesViewModel: ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BranchItemViewModel> _branches = [new BranchItemViewModel()];

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    public BranchesViewModel()
    {
        
    }
}