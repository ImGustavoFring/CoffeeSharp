using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    private static readonly Lazy<CategoryViewModel> _instance = new(() => new CategoryViewModel());

    public static CategoryViewModel Instance => _instance.Value;

    private CategoryViewModel() { }

    [ObservableProperty]
    private ObservableCollection<CategoryItemViewModel> _categories = [];

    [ObservableProperty]
    private string? _searchNameQuery = null;
}