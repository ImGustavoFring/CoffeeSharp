using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.ProductCatalog.Requests;
using Domain.DTOs.ReferenceData.Requests;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class ProductInformationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ProductDtoObservable _productDto;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private ObservableCollection<CategoryItemViewModel> _availableCategories;

    [ObservableProperty]
    private CategoryItemViewModel? _selectedCategory;

    private readonly ProductDto _backupProductDto;

    public ProductInformationWindowViewModel(ProductDtoObservable? productDto, bool isNew)
    {
        _productDto = productDto ?? new ProductDtoObservable();
        _isNew = isNew;
        _backupProductDto = new ProductDto
        {
            Name = _productDto.Name,
            Description = _productDto.Description,
            Price = _productDto.Price,
            CategoryId = _productDto.CategoryId
        };
        
        _availableCategories = new ObservableCollection<CategoryItemViewModel>
        {
            new CategoryItemViewModel(new CategoryDtoObservable { Id = 0, Name = "Без категории" })
        };
        foreach (var category in CategoryViewModel.Instance.Categories)
        {
            _availableCategories.Add(category);
        }
        
        if (_productDto.CategoryId.HasValue)
        {
            SelectedCategory = AvailableCategories.FirstOrDefault(c => c.CategoryDto.Id == _productDto.CategoryId);
        }
        else
        {
            SelectedCategory = AvailableCategories.FirstOrDefault(c => c.CategoryDto.Id == 0); // "Без категории"
        }
    }

    partial void OnSelectedCategoryChanged(CategoryItemViewModel? value)
    {
        ProductDto.CategoryId = value?.CategoryDto.Id == 0 ? null : value?.CategoryDto.Id;
    }

    public async Task<bool> Save()
    {
        try
        {
            if (IsNew)
            {
                var productDto = await HttpClient.Instance.CreateProduct(new CreateProductRequest
                {
                    Name = ProductDto.Name,
                    Description = ProductDto.Description,
                    Price = ProductDto.Price,
                    CategoryId = ProductDto.CategoryId
                });
                ProductDto.Id = productDto.Id;
                ProductDto.Name = productDto.Name;
                ProductDto.Description = productDto.Description;
                ProductDto.Price = productDto.Price;
                ProductDto.CategoryId = productDto.CategoryId;
            }
            else
            {
                await HttpClient.Instance.UpdateProduct(ProductDto.Id, new UpdateProductRequest
                {
                    Id = ProductDto.Id,
                    Name = ProductDto.Name,
                    Description = ProductDto.Description,
                    Price = ProductDto.Price,
                    CategoryId = ProductDto.CategoryId
                });
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
        ProductDto.Name = _backupProductDto.Name;
        ProductDto.Description = _backupProductDto.Description;
        ProductDto.Price = _backupProductDto.Price;
        ProductDto.CategoryId = _backupProductDto.CategoryId;
        SelectedCategory = AvailableCategories.FirstOrDefault(c => c.CategoryDto.Id == (_backupProductDto.CategoryId ?? 0));
    }
}