using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class MenuPresetDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string? _description;

    public MenuPresetDtoObservable() { }

    public MenuPresetDtoObservable(MenuPresetDto dto)
    {
        _id = dto.Id;
        _name = dto.Name;
        _description = dto.Description;
    }

    public MenuPresetDto ToDto()
    {
        return new MenuPresetDto
        {
            Id = Id,
            Name = Name,
            Description = Description
        };
    }
}