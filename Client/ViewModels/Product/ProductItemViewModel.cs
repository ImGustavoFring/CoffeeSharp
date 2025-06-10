using System;
using System.Linq;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class ProductItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private ProductDtoObservable _productDto;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasCategory))]
    private string? _categoryName;

    public bool HasCategory => !string.IsNullOrEmpty(CategoryName);

    public ProductItemViewModel(ProductDtoObservable productDto)
    {
        _productDto = productDto;
        LoadCategoryAsync();
    }

    private async void LoadCategoryAsync()
    {
        if (!_productDto.CategoryId.HasValue)
        {
            CategoryName = null;
            return;
        }

        try
        {
            var existingCategory = CategoryViewModel.Instance.Categories
                .FirstOrDefault(c => c.CategoryDto.Id == _productDto.CategoryId.Value);

            if (existingCategory != null)
            {
                CategoryName = existingCategory.CategoryDto.Name;
            }
            else
            {
                if (!CategoryViewModel.Instance.Categories.Any())
                {
                    var rsp = await HttpClient.Instance.GetAllCategories(null);
                    var categories = rsp.Items;
                    CategoryViewModel.Instance.Categories.Clear();
                    foreach (var category in categories)
                    {
                        CategoryViewModel.Instance.Categories.Add(new CategoryItemViewModel(new CategoryDtoObservable(category)));
                    }
                }

                existingCategory = CategoryViewModel.Instance.Categories
                    .FirstOrDefault(c => c.CategoryDto.Id == _productDto.CategoryId.Value);

                if (existingCategory != null)
                {
                    CategoryName = existingCategory.CategoryDto.Name;
                }
                else
                {
                    var category = await HttpClient.Instance.GetCategoryById(_productDto.CategoryId.Value);
                    CategoryName = category.Name;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке категории для ProductId={_productDto.Id}: {ex.Message}");
        }
    }
}