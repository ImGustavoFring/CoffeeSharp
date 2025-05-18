using System;
using Client.ObservableDTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class ProductItemViewModel(ProductDtoObservable productDto) : ViewModelBase
{
    [ObservableProperty] private ProductDtoObservable _productDto = productDto;
}