using Client.ObservableDTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class MenuPresetItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private MenuPresetDtoObservable _menuPresetDto;

    public MenuPresetItemViewModel(MenuPresetDtoObservable menuPresetDto)
    {
        _menuPresetDto = menuPresetDto;
    }
}