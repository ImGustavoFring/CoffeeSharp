using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class CategoryDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private long? _parentId;

    public CategoryDtoObservable() { }

    public CategoryDtoObservable(CategoryDto dto)
    {
        _id = dto.Id;
        _name = dto.Name;
        _parentId = dto.ParentId;
    }

    public CategoryDto ToDto()
    {
        return new CategoryDto
        {
            Id = Id,
            Name = Name,
            ParentId = ParentId
        };
    }
}