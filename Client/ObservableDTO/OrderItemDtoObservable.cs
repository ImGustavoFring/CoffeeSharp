using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class OrderItemDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private long? _orderId;

    [ObservableProperty]
    private long? _productId;

    [ObservableProperty]
    private long? _employeeId;

    [ObservableProperty]
    private decimal _price;

    [ObservableProperty]
    private long _count;

    [ObservableProperty]
    private DateTime? _startedAt;

    [ObservableProperty]
    private DateTime? _doneAt;

    public OrderItemDtoObservable() { }

    public OrderItemDtoObservable(OrderItemDto dto)
    {
        _id = dto.Id;
        _orderId = dto.OrderId;
        _productId = dto.ProductId;
        _employeeId = dto.EmployeeId;
        _price = dto.Price;
        _count = dto.Count;
        _startedAt = dto.StartedAt;
        _doneAt = dto.DoneAt;
    }

    public OrderItemDto ToDto()
    {
        return new OrderItemDto
        {
            Id = Id,
            OrderId = OrderId,
            ProductId = ProductId,
            EmployeeId = EmployeeId,
            Price = Price,
            Count = Count,
            StartedAt = StartedAt,
            DoneAt = DoneAt
        };
    }
}