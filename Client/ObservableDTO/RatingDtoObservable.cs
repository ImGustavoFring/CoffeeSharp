using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class RatingDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private long _value;

    public RatingDtoObservable() { }

    public RatingDtoObservable(RatingDto dto)
    {
        _id = dto.Id;
        _name = dto.Name;
        _value = dto.Value;
    }

    public RatingDto ToDto()
    {
        return new RatingDto
        {
            Id = Id,
            Name = Name,
            Value = Value
        };
    }
}