using System.Threading.Tasks;
using Client.Services;
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
    
    public async Task<string> GetProductNameAsync()
    {
        if (ProductId == null) return string.Empty;
        var product = await HttpClient.Instance.GetProductById(ProductId ?? 0);
        return product?.Name ?? string.Empty;
    }

    public async Task<string> GetMenuPresetNameAsync()
    {
        if (MenuPresetId == null) return string.Empty;
        var preset = await HttpClient.Instance.GetPresetById(MenuPresetId ?? 0);
        return preset?.Name ?? string.Empty;
    }
}