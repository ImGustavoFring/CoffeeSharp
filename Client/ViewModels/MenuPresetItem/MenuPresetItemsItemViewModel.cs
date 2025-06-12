using System.Threading.Tasks;
using Client.ObservableDTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class MenuPresetItemsItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private MenuPresetItemDtoObservable _menuPresetItemDto;

    [ObservableProperty]
    private string? _presetName;

    [ObservableProperty]
    private string? _productName;

    public MenuPresetItemsItemViewModel(MenuPresetItemDtoObservable menuPresetItemDto)
    {
        _menuPresetItemDto = menuPresetItemDto;
        LoadNamesAsync();
    }

    private async Task LoadNamesAsync()
    {
        PresetName = await MenuPresetItemDto.GetMenuPresetNameAsync();
        ProductName = await MenuPresetItemDto.GetProductNameAsync();
    }
}