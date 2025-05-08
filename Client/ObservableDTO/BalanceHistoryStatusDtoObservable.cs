using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class BalanceHistoryStatusDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _name = string.Empty;

    public BalanceHistoryStatusDtoObservable() { }

    public BalanceHistoryStatusDtoObservable(BalanceHistoryStatusDto dto)
    {
        _id = dto.Id;
        _name = dto.Name;
    }

    public BalanceHistoryStatusDto ToDto()
    {
        return new BalanceHistoryStatusDto
        {
            Id = Id,
            Name = Name
        };
    }
}