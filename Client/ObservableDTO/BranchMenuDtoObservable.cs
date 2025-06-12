using System.Threading.Tasks;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class BranchMenuDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private long? _menuPresetItemId;

    [ObservableProperty]
    private long? _branchId;

    [ObservableProperty]
    private bool _availability;

    public BranchMenuDtoObservable() { }

    public BranchMenuDtoObservable(BranchMenuDto dto)
    {
        _id = dto.Id;
        _menuPresetItemId = dto.MenuPresetItemId;
        _branchId = dto.BranchId;
        _availability = dto.Availability;
    }

    public BranchMenuDto ToDto()
    {
        return new BranchMenuDto
        {
            Id = Id,
            MenuPresetItemId = MenuPresetItemId,
            BranchId = BranchId,
            Availability = Availability
        };
    }
    
    public async Task<string> GetBranchNameAsync()
    {
        if (BranchId == null) return string.Empty;
        var branch = await HttpClient.Instance.GetBranchById(BranchId ?? 0);
        return branch?.Name ?? string.Empty;
    }

    public async Task<string> GetMenuPresetItemNameAsync()
    {
        if (MenuPresetItemId == null) return string.Empty;
        var item = await HttpClient.Instance.GetPresetItemById(MenuPresetItemId ?? 0);
        return $"{await new MenuPresetItemDtoObservable(item).GetProductNameAsync()} ({await new MenuPresetItemDtoObservable(item).GetMenuPresetNameAsync()})";
    }
}