using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class OrderItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private OrderItemDtoObservable _orderItemDto;

    [ObservableProperty]
    private string _displayName;

    [ObservableProperty]
    private EmployeeDtoObservable? _selectedEmployee;

    [ObservableProperty]
    private bool _canToggleComplete;

    [ObservableProperty]
    private bool _isModified;

    [ObservableProperty]
    private bool _isManager;

    [ObservableProperty]
    private ObservableCollection<EmployeeDtoObservable> _employees;

    [ObservableProperty]
    private string _completeButtonText;

    public DateTime? OriginalDoneAt { get; }

    private readonly long? _currentEmployeeId;

    private OrderItemViewModel(OrderItemDtoObservable orderItemDto, string displayName, bool isManager, ObservableCollection<EmployeeDtoObservable> employees, long? currentEmployeeId)
    {
        _orderItemDto = orderItemDto;
        _displayName = displayName;
        _isManager = isManager;
        _employees = employees;
        _currentEmployeeId = currentEmployeeId;
        _canToggleComplete = isManager || currentEmployeeId == orderItemDto.EmployeeId;
        OriginalDoneAt = orderItemDto.DoneAt;
        _completeButtonText = orderItemDto.DoneAt.HasValue ? "Не готово" : "Готово";
    }

    public static async Task<OrderItemViewModel> CreateAsync(OrderItemDto orderItemDto, bool isManager, ObservableCollection<EmployeeDtoObservable> employees)
    {
        // Формирование отображаемого имени позиции
        var displayName = "Неизвестный продукт";
        if (orderItemDto.ProductId.HasValue)
        {
            var product = await HttpClient.Instance.GetProductById(orderItemDto.ProductId.Value);
            displayName = $"{product?.Name ?? "Неизвестный продукт"} x{orderItemDto.Count}";
        }

        var auth = AuthService.Load();
        long.TryParse(orderItemDto.EmployeeId.ToString(), out var employeeId);
        var viewModel = new OrderItemViewModel(new OrderItemDtoObservable(orderItemDto), displayName, isManager, employees, employeeId);

        // Установка текущего сотрудника для ComboBox менеджера
        if (isManager && orderItemDto.EmployeeId.HasValue)
        {
            var employee = await HttpClient.Instance.GetEmployeeById(orderItemDto.EmployeeId.Value);
            if (employee != null)
            {
                viewModel.SelectedEmployee = employees.FirstOrDefault(e => e.Id == employee.Id) ?? new EmployeeDtoObservable(employee);
            }
        }

        return viewModel;
    }

    partial void OnSelectedEmployeeChanged(EmployeeDtoObservable? value)
    {
        IsModified = true;
    }

    partial void OnOrderItemDtoChanged(OrderItemDtoObservable value)
    {
        CompleteButtonText = value.DoneAt.HasValue ? "Отменить готовность" : "Пометить готовым";
        IsModified = true;
    }

    [RelayCommand]
    private void ToggleComplete()
    {
        if (OrderItemDto.DoneAt.HasValue)
        {
            OrderItemDto.DoneAt = null;
        }
        else
        {
            OrderItemDto.DoneAt = DateTime.Now;
        }
        IsModified = true;
    }
}