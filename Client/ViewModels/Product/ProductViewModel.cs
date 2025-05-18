using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class ProductViewModel : ViewModelBase
{
    private static readonly Lazy<ProductViewModel> _instance = new(() => new ProductViewModel());

    public static ProductViewModel Instance => _instance.Value;

    private ProductViewModel() { }
    
    [ObservableProperty]
    private ObservableCollection<ProductItemViewModel> _products = new();
    
    [ObservableProperty]
    private string? _searchNameQuery = null;
}