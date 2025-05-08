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
}