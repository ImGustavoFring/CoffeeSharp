using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

    private MainOrdersViewModel() { }
    
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SortedOrderShortItemViewModels))]
    private ObservableCollection<OrderShortItemViewModel> _orderShortItemViewModels = [new OrderShortItemViewModel()];

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SortedOrderShortItemViewModels))]
    private OrderShortItemViewModelsSortingTypeEnum _currentSortingType;

    public ObservableCollection<OrderShortItemViewModel> SortedOrderShortItemViewModels => CurrentSortingType switch
    {
        OrderShortItemViewModelsSortingTypeEnum.Default => OrderShortItemViewModels,
        OrderShortItemViewModelsSortingTypeEnum.TimeAscending => new
            ObservableCollection<OrderShortItemViewModel>(
                OrderShortItemViewModels.OrderBy(x => x.OrderDtoItem.CreatedAt)),
        OrderShortItemViewModelsSortingTypeEnum.TimeDescending => new
            ObservableCollection<OrderShortItemViewModel>(
                OrderShortItemViewModels.OrderByDescending(x => x.OrderDtoItem.CreatedAt)),
        _ => OrderShortItemViewModels
    };
    
    public void SetDefaultSortingType()
    {
        CurrentSortingType = OrderShortItemViewModelsSortingTypeEnum.Default;
    }
    
    public void SetTimeAscendingSortingType()
    {
        CurrentSortingType = OrderShortItemViewModelsSortingTypeEnum.TimeAscending;
    }
    
    public void SetTimeDescendingSortingType()
    {
        CurrentSortingType = OrderShortItemViewModelsSortingTypeEnum.TimeDescending;
    }
}
