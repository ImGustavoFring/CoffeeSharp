using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Branch.Requests;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class BranchMenuItemsItemInformationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private BranchMenuDtoObservable _branchMenuDto;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private ObservableCollection<MenuPresetDtoObservable> _availablePresets;

    [ObservableProperty]
    private ObservableCollection<BranchDtoObservable> _availableBranches;

    [ObservableProperty]
    private MenuPresetDtoObservable? _selectedPreset;

    [ObservableProperty]
    private BranchDtoObservable? _selectedBranch;

    private readonly BranchMenuDto _backupBranchMenuDto;

    public BranchMenuItemsItemInformationWindowViewModel(BranchMenuDtoObservable? branchMenuDto, bool isNew)
    {
        _branchMenuDto = branchMenuDto ?? new BranchMenuDtoObservable();
        _isNew = isNew;
        _backupBranchMenuDto = new BranchMenuDto
        {
            Id = _branchMenuDto.Id,
            MenuPresetItemId = _branchMenuDto.MenuPresetItemId,
            BranchId = _branchMenuDto.BranchId
        };

        _availablePresets = new ObservableCollection<MenuPresetDtoObservable>();
        _availableBranches = new ObservableCollection<BranchDtoObservable>();
        LoadPresetsAsync();
        LoadBranchesAsync();
    }

    private async void LoadPresetsAsync()
    {
        try
        {
            var presets = await HttpClient.Instance.GetAllPresets();
            AvailablePresets.Clear();
            foreach (var preset in presets)
            {
                AvailablePresets.Add(new MenuPresetDtoObservable(preset));
            }
            if (_branchMenuDto.MenuPresetItemId.HasValue)
            {
                var presetItem = await HttpClient.Instance.GetPresetItemById(_branchMenuDto.MenuPresetItemId.Value);
                if (presetItem?.MenuPresetId != null)
                {
                    SelectedPreset = AvailablePresets.FirstOrDefault(p => p.Id == presetItem.MenuPresetId);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке пресетов: {ex.Message}");
        }
    }

    private async void LoadBranchesAsync()
    {
        try
        {
            var (branches, _) = await HttpClient.Instance.GetAllBranches();
            AvailableBranches.Clear();
            foreach (var branch in branches)
            {
                AvailableBranches.Add(new BranchDtoObservable(branch));
            }
            if (_branchMenuDto.BranchId.HasValue)
            {
                SelectedBranch = AvailableBranches.FirstOrDefault(b => b.Id == _branchMenuDto.BranchId);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке филиалов: {ex.Message}");
        }
    }

    partial void OnSelectedPresetChanged(MenuPresetDtoObservable? value)
    {
        if (value != null)
        {
            Task.Run(async () =>
            {
                var items = await HttpClient.Instance.GetPresetItems(menuPresetId: value.Id);
                BranchMenuDto.MenuPresetItemId = items.FirstOrDefault()?.Id;
            });
        }
    }

    partial void OnSelectedBranchChanged(BranchDtoObservable? value)
    {
        BranchMenuDto.BranchId = value?.Id;
    }

    public async Task<bool> Save()
    {
        try
        {
            if (!BranchMenuDto.BranchId.HasValue || !BranchMenuDto.MenuPresetItemId.HasValue)
            {
                await DialogsHelper.ShowError("Необходимо выбрать пресет и филиал");
                return false;
            }
            if (IsNew)
            {
                await HttpClient.Instance.AssignMenuPresetToBranch(BranchMenuDto.BranchId.Value, new AssignMenuPresetRequest
                {
                    MenuPresetId = (await HttpClient.Instance.GetPresetItemById(BranchMenuDto.MenuPresetItemId.Value)).MenuPresetId!.Value
                });
                var branchMenus = await HttpClient.Instance.GetBranchMenus(branchId: BranchMenuDto.BranchId, menuPresetId: BranchMenuDto.MenuPresetItemId);
                var createdMenu = branchMenus.FirstOrDefault();
                if (createdMenu != null)
                {
                    BranchMenuDto.Id = createdMenu.Id;
                    BranchMenuDto.MenuPresetItemId = createdMenu.MenuPresetItemId;
                    BranchMenuDto.BranchId = createdMenu.BranchId;
                }
            }
            else
            {
                await HttpClient.Instance.DeleteBranch(BranchMenuDto.BranchId!.Value);
                await HttpClient.Instance.AssignMenuPresetToBranch(BranchMenuDto.BranchId.Value, new AssignMenuPresetRequest
                {
                    MenuPresetId = (await HttpClient.Instance.GetPresetItemById(BranchMenuDto.MenuPresetItemId.Value)).MenuPresetId!.Value
                });
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении привязки: {ex.Message}");
            return false;
        }
    }

    public void Cancel()
    {
        BranchMenuDto.MenuPresetItemId = _backupBranchMenuDto.MenuPresetItemId;
        BranchMenuDto.BranchId = _backupBranchMenuDto.BranchId;
        BranchMenuDto.Id = _backupBranchMenuDto.Id;
        SelectedPreset = AvailablePresets.FirstOrDefault(p => p.Id == _backupBranchMenuDto.MenuPresetItemId);
        SelectedBranch = AvailableBranches.FirstOrDefault(b => b.Id == _backupBranchMenuDto.BranchId);
    }
}