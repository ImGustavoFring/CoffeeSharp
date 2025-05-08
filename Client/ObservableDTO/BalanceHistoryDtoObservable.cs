using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class BalanceHistoryDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private long? _clientId;

    [ObservableProperty]
    private decimal _sum;

    [ObservableProperty]
    private DateTime _createdAt;

    [ObservableProperty]
    private DateTime? _finishedAt;

    [ObservableProperty]
    private long? _balanceHistoryStatusId;

    public BalanceHistoryDtoObservable() { }

    public BalanceHistoryDtoObservable(BalanceHistoryDto dto)
    {
        _id = dto.Id;
        _clientId = dto.ClientId;
        _sum = dto.Sum;
        _createdAt = dto.CreatedAt;
        _finishedAt = dto.FinishedAt;
        _balanceHistoryStatusId = dto.BalanceHistoryStatusId;
    }

    public BalanceHistoryDto ToDto()
    {
        return new BalanceHistoryDto
        {
            Id = Id,
            ClientId = ClientId,
            Sum = Sum,
            CreatedAt = CreatedAt,
            FinishedAt = FinishedAt,
            BalanceHistoryStatusId = BalanceHistoryStatusId
        };
    }
}