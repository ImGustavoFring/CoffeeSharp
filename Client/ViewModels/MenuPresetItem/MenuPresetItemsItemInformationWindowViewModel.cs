using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Menu.Requests;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class MenuPresetItemsItemInformationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private MenuPresetItemDtoObservable _menuPresetItemDto;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private ObservableCollection<MenuPresetDtoObservable> _availablePresets;

    [ObservableProperty]
    private ObservableCollection<ProductDtoObservable> _availableProducts;

    [ObservableProperty]
    private MenuPresetDtoObservable? _selectedPreset;

    [ObservableProperty]
    private ProductDtoObservable? _selectedProduct;

    private readonly MenuPresetItemDto _backupMenuPresetItemDto;

    public MenuPresetItemsItemInformationWindowViewModel(MenuPresetItemDtoObservable? menuPresetItemDto, bool isNew)
    {
        _menuPresetItemDto = menuPresetItemDto ?? new MenuPresetItemDtoObservable();
        _isNew = isNew;
        _backupMenuPresetItemDto = new MenuPresetItemDto
        {
            Id = _menuPresetItemDto.Id,
            MenuPresetId = _menuPresetItemDto.MenuPresetId,
            ProductId = _menuPresetItemDto.ProductId
        };

        _availablePresets = new ObservableCollection<MenuPresetDtoObservable>();
        _availableProducts = new ObservableCollection<ProductDtoObservable>();
        LoadPresetsAsync();
        LoadProductsAsync();
    }

    private async void LoadPresetsAsync()
    {
        try
        {
            var presets = await HttpClient.Instance.GetAllPresets();
            AvailablePresets.Clear();
            foreach (var preset in presets)
            {
                AvailablePresets.Add(new MenuPresetDtoObservable(preset));
            }
            if (_menuPresetItemDto.MenuPresetId.HasValue)
            {
                SelectedPreset = AvailablePresets.FirstOrDefault(p => p.Id == _menuPresetItemDto.MenuPresetId);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке пресетов: {ex.Message}");
        }
    }

    private async void LoadProductsAsync()
    {
        try
        {
            var products = (await HttpClient.Instance.GetAllProducts()).Items;
            AvailableProducts.Clear();
            foreach (var product in products)
            {
                AvailableProducts.Add(new ProductDtoObservable(product));
            }
            if (_menuPresetItemDto.ProductId.HasValue)
            {
                SelectedProduct = AvailableProducts.FirstOrDefault(p => p.Id == _menuPresetItemDto.ProductId);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке продуктов: {ex.Message}");
        }
    }

    partial void OnSelectedPresetChanged(MenuPresetDtoObservable? value)
    {
        MenuPresetItemDto.MenuPresetId = value?.Id;
    }

    partial void OnSelectedProductChanged(ProductDtoObservable? value)
    {
        MenuPresetItemDto.ProductId = value?.Id;
    }

    public async Task<bool> Save()
    {
        try
        {
            if (!MenuPresetItemDto.MenuPresetId.HasValue || !MenuPresetItemDto.ProductId.HasValue)
            {
                await DialogsHelper.ShowError("Необходимо выбрать пресет и продукт");
                return false;
            }
            if (IsNew)
            {
                var itemDto = await HttpClient.Instance.CreatePresetItem(new CreateMenuPresetItemRequest
                {
                    MenuPresetId = MenuPresetItemDto.MenuPresetId.Value,
                    ProductId = MenuPresetItemDto.ProductId.Value
                });
                MenuPresetItemDto.Id = itemDto.Id;
                MenuPresetItemDto.MenuPresetId = itemDto.MenuPresetId;
                MenuPresetItemDto.ProductId = itemDto.ProductId;
            }
            else
            {
                await HttpClient.Instance.UpdatePresetItem(MenuPresetItemDto.Id, new UpdateMenuPresetItemRequest
                {
                    Id = MenuPresetItemDto.Id,
                    MenuPresetId = MenuPresetItemDto.MenuPresetId.Value,
                    ProductId = MenuPresetItemDto.ProductId.Value
                });
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении элемента пресета: {ex.Message}");
            return false;
        }
    }

    public void Cancel()
    {
        MenuPresetItemDto.MenuPresetId = _backupMenuPresetItemDto.MenuPresetId;
        MenuPresetItemDto.ProductId = _backupMenuPresetItemDto.ProductId;
        MenuPresetItemDto.Id = _backupMenuPresetItemDto.Id;
        SelectedPreset = AvailablePresets.FirstOrDefault(p => p.Id == _backupMenuPresetItemDto.MenuPresetId);
        SelectedProduct = AvailableProducts.FirstOrDefault(p => p.Id == _backupMenuPresetItemDto.ProductId);
    }
}