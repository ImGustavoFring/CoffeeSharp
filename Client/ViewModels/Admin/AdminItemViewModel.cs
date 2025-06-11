using Client.ObservableDTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class AdminItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private AdminDtoObservable _adminDto;

    public AdminItemViewModel(AdminDtoObservable adminDto)
    {
        _adminDto = adminDto;
    }
}