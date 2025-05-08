using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class MenuPresetItemDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private long? _productId;

    [ObservableProperty]
    private long? _menuPresetId;

    public MenuPresetItemDtoObservable() { }

    public MenuPresetItemDtoObservable(MenuPresetItemDto dto)
    {
        _id = dto.Id;
        _productId = dto.ProductId;
        _menuPresetId = dto.MenuPresetId;
    }

    public MenuPresetItemDto ToDto()
    {
        return new MenuPresetItemDto
        {
            Id = Id,
            ProductId = ProductId,
            MenuPresetId = MenuPresetId
        };
    }
}