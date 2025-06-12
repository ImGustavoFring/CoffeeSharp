using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.DTOs;

namespace Client.ViewModels;

public enum OrderShortItemViewModelsSortingTypeEnum
{
    Default,
    TimeAscending,
    TimeDescending
}

public partial class MainOrdersViewModel : ViewModelBase
{
    private static readonly Lazy<MainOrdersViewModel> _instance = new(() => new MainOrdersViewModel());

    public static MainOrdersViewModel Instance => _instance.Value;

    private MainOrdersViewModel()
    {
        LoadOrdersAsync();
    }
    
    [ObservableProperty]
    private DateTime _currentDateTime = DateTime.Now;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SortedOrderShortItemViewModels))]
    private ObservableCollection<OrderShortItemViewModel> _orderShortItemViewModels = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SortedOrderShortItemViewModels))]
    private OrderShortItemViewModelsSortingTypeEnum _currentSortingType;

    public ObservableCollection<OrderShortItemViewModel> SortedOrderShortItemViewModels => CurrentSortingType switch
    {
        OrderShortItemViewModelsSortingTypeEnum.Default => OrderShortItemViewModels,
        OrderShortItemViewModelsSortingTypeEnum.TimeAscending => new ObservableCollection<OrderShortItemViewModel>(
            OrderShortItemViewModels.OrderBy(x => x.OrderDtoItem.CreatedAt)),
        OrderShortItemViewModelsSortingTypeEnum.TimeDescending => new ObservableCollection<OrderShortItemViewModel>(
            OrderShortItemViewModels.OrderByDescending(x => x.OrderDtoItem.CreatedAt)),
        _ => OrderShortItemViewModels
    };

    [RelayCommand]
    private async Task LoadOrdersAsync()
    {
        try
        {
            var auth = AuthService.Load();
            long? branchId = auth.UserType == "admin" ? null : auth.BranchId;
            var (orders, _) = await HttpClient.Instance.GetOrders(branchId: branchId);

            OrderShortItemViewModels.Clear();
            foreach (var order in orders)
            {
                var viewModel = await OrderShortItemViewModel.CreateAsync(order);
                OrderShortItemViewModels.Add(viewModel);
            }
        }
        catch
        {

        }
    }

    [RelayCommand]
    private void SetDefaultSortingType()
    {
        CurrentSortingType = OrderShortItemViewModelsSortingTypeEnum.Default;
    }
    
    [RelayCommand]
    private void SetTimeAscendingSortingType()
    {
        CurrentSortingType = OrderShortItemViewModelsSortingTypeEnum.TimeAscending;
    }
    
    [RelayCommand]
    private void SetTimeDescendingSortingType()
    {
        CurrentSortingType = OrderShortItemViewModelsSortingTypeEnum.TimeDescending;
    }
}