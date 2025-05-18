using System;
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

public partial class ProductInformationWindowViewModel(ProductDtoObservable? productDto, bool isNew) : ViewModelBase
{
    [ObservableProperty] private ProductDtoObservable _productDto = productDto ?? new ProductDtoObservable();
    [ObservableProperty] private bool _isNew = isNew;

    private readonly ProductDto _backupProductDto = new ProductDto()
    {
        Name = productDto?.Name ?? string.Empty,
        Description = productDto?.Description,
        Price = productDto?.Price ?? 0
    };

    public async Task<bool> Save()
    {
        try
        {
            if (IsNew)
            {
                var productDto = await HttpClient.Instance.CreateProduct(new CreateProductRequest()
                {
                    Name = ProductDto.Name,
                    Description = ProductDto.Description,
                    Price = ProductDto.Price,
                    CategoryId = 1
                });
                ProductDto.Id = productDto.Id;
                ProductDto.Name = productDto.Name;
                ProductDto.Description = productDto.Description;
                ProductDto.Price = productDto.Price;
            }
            else
            {
                await HttpClient.Instance.UpdateProduct(ProductDto.Id, new UpdateProductRequest()
                {
                    Id = ProductDto.Id,
                    Name = ProductDto.Name,
                    Description = ProductDto.Description,
                    Price = ProductDto.Price,
                    CategoryId = 1
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
    }
}