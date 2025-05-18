using System;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class CategoryItemViewModel(CategoryDtoObservable categoryDto) : ViewModelBase
{
    [ObservableProperty] private CategoryDtoObservable _categoryDto = categoryDto;
    
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HaveParentCategory))] private CategoryDtoObservable? _parentCategoryDto = null;
    
    public bool HaveParentCategory => ParentCategoryDto != null;

    public async Task GetParentCategory()
    {
        try
        {
            ParentCategoryDto = new CategoryDtoObservable(await HttpClient.Instance.GetCategoryById(CategoryDto.Id));
        }
        catch
        {
            // ignored
        }
    }
}
