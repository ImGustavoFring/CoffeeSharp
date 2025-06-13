using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class OrderInformationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private OrderDtoObservable _orderDto;

    [ObservableProperty]
    private string? _clientName;

    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _orderItems = new();

    private readonly ObservableCollection<EmployeeDtoObservable> _employees = new();
    private readonly bool _isManager;
    private readonly OrderDtoObservable _backupOrderDto;

    public OrderInformationWindowViewModel(OrderDtoObservable orderDto)
    {
        _orderDto = orderDto;
        _backupOrderDto = new OrderDtoObservable(orderDto.ToDto());
        _isManager = AuthService.Instance.MainWindowViewModel.IsHighCook;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            if (OrderDto.ClientId.HasValue)
            {
                var client = await HttpClient.Instance.GetClientById(OrderDto.ClientId.Value);
                ClientName = client?.Name ?? "Неизвестный клиент";
            }
            
            var (orderItems, _) = await HttpClient.Instance.GetOrderItems(orderId: OrderDto.Id);
            OrderItems.Clear();
            foreach (var item in orderItems)
            {
                var itemVm = await OrderItemViewModel.CreateAsync(item, _isManager, _employees);
                OrderItems.Add(itemVm);
            }
            
            if (_isManager)
            {
                var (employees, _) = await HttpClient.Instance.GetAllEmployees(branchId: OrderDto.BranchId);
                _employees.Clear();
                foreach (var emp in employees)
                {
                    _employees.Add(new EmployeeDtoObservable(emp));
                }
            }
        }
        catch
        {
            await DialogsHelper.ShowError("Ошибка при загрузке данных заказа");
        }
    }

    public async Task<bool> Save()
    {
        try
        {
            foreach (var item in OrderItems)
            {
                if (item.IsModified)
                {
                    // Переназначение сотрудника для менеджера
                    if (_isManager && item.SelectedEmployee?.Id != item.OrderItemDto.EmployeeId)
                    {
                        await HttpClient.Instance.ReassignOrderItem(
                            item.OrderItemDto.Id,
                            item.SelectedEmployee?.Id ?? throw new InvalidOperationException("Сотрудник не выбран"));
                    }

                    // Обновление статуса готовности
                    if (item.OrderItemDto.DoneAt.HasValue != item.OriginalDoneAt.HasValue)
                    {
                        if (item.OrderItemDto.DoneAt.HasValue)
                        {
                            await HttpClient.Instance.CompleteOrderItem(item.OrderItemDto.Id);
                        }
                        else
                        {
                            await DialogsHelper.ShowError("Невозможно отменить завершение позиции");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Cancel()
    {
        OrderDto.ClientId = _backupOrderDto.ClientId;
        OrderDto.ClientNote = _backupOrderDto.ClientNote;
    }
}