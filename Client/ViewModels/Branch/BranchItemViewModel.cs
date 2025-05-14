using System;
using Client.ObservableDTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class BranchItemViewModel(BranchDtoObservable branchDto) : ViewModelBase
{
    [ObservableProperty] private BranchDtoObservable _branchDto = branchDto;
    
}
