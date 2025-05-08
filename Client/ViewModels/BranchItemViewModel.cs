using Client.ObservableDTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class BranchItemViewModel(BranchDtoObservable branchDto) : ViewModelBase
{
    [ObservableProperty] private BranchDtoObservable _branchDto = branchDto;

    public BranchItemViewModel() : this(new BranchDtoObservable()
    {
        Id = 1,
        Name = "Великий филиал №1",
        Address = "г. Курган, ул. Коли Мяготина, д. 123"
    })
    {
    }
}