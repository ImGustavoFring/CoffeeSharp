using Client.ObservableDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using Client.Services;

namespace Client.ViewModels;

public partial class BranchMenuItemsItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private BranchMenuDtoObservable _branchMenuDto;

    [ObservableProperty]
    private string? _presetName;

    [ObservableProperty]
    private string? _branchName;

    public BranchMenuItemsItemViewModel(BranchMenuDtoObservable branchMenuDto)
    {
        _branchMenuDto = branchMenuDto;
        LoadNamesAsync();
    }

    private async Task LoadNamesAsync()
    {
        if (BranchMenuDto.MenuPresetItemId.HasValue)
        {
            var presetItem = await HttpClient.Instance.GetPresetItemById(BranchMenuDto.MenuPresetItemId.Value);
            if (presetItem != null && presetItem.MenuPresetId.HasValue)
            {
                var preset = await HttpClient.Instance.GetPresetById(presetItem.MenuPresetId.Value);
                PresetName = preset?.Name;
            }
        }
        if (BranchMenuDto.BranchId.HasValue)
        {
            var branch = await HttpClient.Instance.GetBranchById(BranchMenuDto.BranchId.Value);
            BranchName = branch?.Name;
        }
    }
}