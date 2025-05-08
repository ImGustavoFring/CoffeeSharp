using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class ProductDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private decimal _price;

    [ObservableProperty]
    private long? _categoryId;

    public ProductDtoObservable() { }

    public ProductDtoObservable(ProductDto dto)
    {
        _id = dto.Id;
        _name = dto.Name;
        _description = dto.Description;
        _price = dto.Price;
        _categoryId = dto.CategoryId;
    }

    public ProductDto ToDto()
    {
        return new ProductDto
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Price = Price,
            CategoryId = CategoryId
        };
    }
}