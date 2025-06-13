using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs;

namespace Client.ViewModels;

public partial class OrderShortItemViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CreateTime))]
    private OrderDtoObservable _orderDtoItem;

    [ObservableProperty]
    private string? _clientName;

    [ObservableProperty]
    private ObservableCollection<string> _employeeNames = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProductNamesDisplay))]
    private ObservableCollection<string> _productNames = new();

    public string CreateTime => OrderDtoItem.CreatedAt.ToString("HH:mm");

    public string ProductNamesDisplay => string.Join(", ", ProductNames);

    private OrderShortItemViewModel(OrderDtoObservable orderDtoItem)
    {
        _orderDtoItem = orderDtoItem;
    }

    public static async Task<OrderShortItemViewModel> CreateAsync(OrderDtoObservable orderDto)
    {
        var viewModel = new OrderShortItemViewModel(orderDto);
        await viewModel.LoadDetailsAsync();
        return viewModel;
    }

    private async Task LoadDetailsAsync()
    {
        try
        {
            if (OrderDtoItem.ClientId.HasValue)
            {
                var client = await HttpClient.Instance.GetClientById(OrderDtoItem.ClientId.Value);
                ClientName = client?.Name ?? "Неизвестный клиент";
            }

            var (orderItems, _) = await HttpClient.Instance.GetOrderItems(orderId: OrderDtoItem.Id);
            EmployeeNames.Clear();
            foreach (var item in orderItems)
            {
                if (item.EmployeeId.HasValue)
                {
                    var employee = await HttpClient.Instance.GetEmployeeById(item.EmployeeId.Value);
                    if (employee != null && !EmployeeNames.Contains(employee.Name))
                    {
                        EmployeeNames.Add(employee.Name);
                    }
                }
            }
            if (!EmployeeNames.Any())
            {
                EmployeeNames.Add("Не назначено");
            }

            ProductNames.Clear();
            foreach (var item in orderItems)
            {
                if (item.ProductId.HasValue)
                {
                    var product = await HttpClient.Instance.GetProductById(item.ProductId.Value);
                    if (product != null)
                    {
                        ProductNames.Add($"{product.Name} x{item.Count}");
                    }
                }
            }
            if (!ProductNames.Any())
            {
                ProductNames.Add("Нет продуктов");
            }
        }
        catch
        {
            ClientName = "Ошибка загрузки";
            EmployeeNames.Clear();
            EmployeeNames.Add("Ошибка загрузки");
            ProductNames.Clear();
            ProductNames.Add("Ошибка загрузки");
        }
    }
}