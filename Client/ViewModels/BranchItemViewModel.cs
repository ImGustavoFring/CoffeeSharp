using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;
using Tmds.DBus.Protocol;

namespace Client.ViewModels;

public partial class BranchItemViewModel(BranchDto branchDto) : ViewModelBase
{
    [ObservableProperty] private BranchDto _branchDto = branchDto;
    [ObservableProperty] private string _address = "г. Курган, ул. Коли Мяготина, д. 123";

    public BranchItemViewModel() : this(new BranchDto()
    {
        Id = 1,
        Name = "Великий филиал №1",
        Address = "г. Курган, ул. Коли Мяготина, д. 123"
    })
    {
    }
}