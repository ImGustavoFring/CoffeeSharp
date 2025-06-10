using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.ProductCatalog.Requests;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class CategoryInformationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private CategoryDtoObservable _categoryDto;

    [ObservableProperty]
    private ObservableCollection<CategoryItemViewModel> _categories = CategoryViewModel.Instance.Categories;

    [ObservableProperty]
    private ObservableCollection<CategoryItemViewModel> _availableCategories;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private CategoryItemViewModel? _selectedParentCategory;

    private readonly CategoryDto _backupCategoryDto;

    public CategoryInformationWindowViewModel(CategoryDtoObservable? categoryDto, bool isNew)
    {
        _categoryDto = categoryDto ?? new CategoryDtoObservable();
        _isNew = isNew;
        _backupCategoryDto = new CategoryDto
        {
            Name = _categoryDto.Name,
            ParentId = _categoryDto.ParentId
        };

        // Инициализация AvailableCategories с опцией "Без родительской категории"
        _availableCategories = new ObservableCollection<CategoryItemViewModel>
        {
            new CategoryItemViewModel(new CategoryDtoObservable { Id = 0, Name = "Без родительской категории" })
        };
        foreach (var category in _categories.Where(c => c.CategoryDto.Id != _categoryDto.Id))
        {
            _availableCategories.Add(category);
        }

        // Инициализация выбранной родительской категории
        if (!_isNew && _categoryDto.ParentId.HasValue)
        {
            SelectedParentCategory = AvailableCategories.FirstOrDefault(c => c.CategoryDto.Id == _categoryDto.ParentId);
        }
        else
        {
            SelectedParentCategory = AvailableCategories.FirstOrDefault(c => c.CategoryDto.Id == 0); // "Без родительской категории"
        }
    }

    partial void OnSelectedParentCategoryChanged(CategoryItemViewModel? value)
    {
        CategoryDto.ParentId = value?.CategoryDto.Id == 0 ? null : value?.CategoryDto.Id;
    }

    public async Task<bool> Save()
    {
        try
        {
            if (IsNew)
            {
                var categoryDto = await HttpClient.Instance.CreateCategory(new CreateCategoryRequest
                {
                    Name = CategoryDto.Name,
                    ParentId = CategoryDto.ParentId
                });
                CategoryDto.Id = categoryDto.Id;
                CategoryDto.Name = categoryDto.Name;
                CategoryDto.ParentId = categoryDto.ParentId;
            }
            else
            {
                await HttpClient.Instance.UpdateCategory(CategoryDto.Id, new UpdateCategoryRequest
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
        CategoryDto.ParentId = _backupCategoryDto.ParentId;
        SelectedParentCategory = AvailableCategories.FirstOrDefault(c => c.CategoryDto.Id == (_backupCategoryDto.ParentId ?? 0));
    }
}