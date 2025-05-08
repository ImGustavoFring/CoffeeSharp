using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class OrderDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private long? _clientId;

    [ObservableProperty]
    private string? _clientNote;

    [ObservableProperty]
    private DateTime _createdAt;

    [ObservableProperty]
    private DateTime? _doneAt;

    [ObservableProperty]
    private DateTime? _finishedAt;

    [ObservableProperty]
    private DateTime? _expectedIn;

    [ObservableProperty]
    private long? _branchId;

    public OrderDtoObservable() { }

    public OrderDtoObservable(OrderDto dto)
    {
        _id = dto.Id;
        _clientId = dto.ClientId;
        _clientNote = dto.ClientNote;
        _createdAt = dto.CreatedAt;
        _doneAt = dto.DoneAt;
        _finishedAt = dto.FinishedAt;
        _expectedIn = dto.ExpectedIn;
        _branchId = dto.BranchId;
    }

    public OrderDto ToDto()
    {
        return new OrderDto
        {
            Id = Id,
            ClientId = ClientId,
            ClientNote = ClientNote,
            CreatedAt = CreatedAt,
            DoneAt = DoneAt,
            FinishedAt = FinishedAt,
            ExpectedIn = ExpectedIn,
            BranchId = BranchId
        };
    }
}