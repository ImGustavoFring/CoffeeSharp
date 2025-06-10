using System;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class CategoryItemViewModel : ViewModelBase
{
    [ObservableProperty] private CategoryDtoObservable _categoryDto;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HaveParentCategory))]
    private CategoryDtoObservable? _parentCategoryDto = null;

    public bool HaveParentCategory => ParentCategoryDto != null;

    public CategoryItemViewModel(CategoryDtoObservable categoryDto)
    {
        _categoryDto = categoryDto;
        GetParentCategory();
    }

    public async Task GetParentCategory()
    {
        try
        {
            if (CategoryDto.ParentId != null)
            {
                ParentCategoryDto = new CategoryDtoObservable(await HttpClient.Instance.GetCategoryById(CategoryDto.ParentId ?? 0));
            }
        }
        catch
        {
            // ignored
        }
    }
}