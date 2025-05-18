using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.ProductCatalog.Requests;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class CategoryInformationWindowViewModel(CategoryDtoObservable? categoryDto, bool isNew) : ViewModelBase
{
    [ObservableProperty] private CategoryDtoObservable _categoryDto = categoryDto ?? new CategoryDtoObservable();
    
    [ObservableProperty]
    private ObservableCollection<CategoryItemViewModel> _categories = CategoryViewModel.Instance.Categories;
    
    [ObservableProperty] private bool _isNew = isNew;
    
    private readonly CategoryDto _backupCategoryDto = new CategoryDto()
    {
        Name = categoryDto != null ? categoryDto.Name : string.Empty
    };

    public async Task<bool> Save()
    {
        try
        {
            if (IsNew)
            {
                var categoryDto = await HttpClient.Instance.CreateCategory(new CreateCategoryRequest()
                {
                    Name = CategoryDto.Name
                });
                CategoryDto.Id = categoryDto.Id;
                CategoryDto.Name = categoryDto.Name;
            }
            else
            {
                await HttpClient.Instance.UpdateCategory(CategoryDto.Id, new UpdateCategoryRequest()
                {
                    Id = CategoryDto.Id,
                    Name = CategoryDto.Name,
                    ParentId = CategoryDto.ParentId
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
        CategoryDto.Name = _backupCategoryDto.Name;
    }
}