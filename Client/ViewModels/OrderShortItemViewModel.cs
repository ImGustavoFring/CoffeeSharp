using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Swift;
using Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs;

namespace Client.ViewModels;

public partial class OrderShortItemViewModel: ViewModelBase
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(CreateTime))] private OrderDto _orderDtoItem;

    [ObservableProperty] private string _clientName;
    
    [ObservableProperty] private ObservableCollection<string> _employeeNames;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(ProductNamesDisplay))] private ObservableCollection<string> _productNames;

    public string CreateTime => OrderDtoItem.CreatedAt.ToString("HH:mm");
    
    public string ProductNamesDisplay => string.Join(", ", ProductNames);

    public OrderShortItemViewModel()
    {
        _orderDtoItem = new OrderDto
        {
            Id = 1234567890,
            ClientId = 1,
            ClientNote = "Латте x2, Пирожок с чем-то.",
            CreatedAt = DateTime.Today.AddHours(15).AddMinutes(48),
            DoneAt = null,
            FinishedAt = null,
            ExpectedIn = DateTime.Today.AddHours(16),
            BranchId = 101
        };
        _clientName = "Иванов И.И.";
        _employeeNames = ["Неиванов Н.Н.", "Петров И.В."];
        _productNames = ["Латте x2", "Пирожок"];
    }
}
