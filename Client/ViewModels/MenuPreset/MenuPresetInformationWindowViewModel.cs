using System;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Menu.Requests;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class MenuPresetInformationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private MenuPresetDtoObservable _menuPresetDto;

    [ObservableProperty]
    private bool _isNew;

    private readonly MenuPresetDto _backupMenuPresetDto;

    public MenuPresetInformationWindowViewModel(MenuPresetDtoObservable? menuPresetDto, bool isNew)
    {
        _menuPresetDto = menuPresetDto ?? new MenuPresetDtoObservable();
        _isNew = isNew;
        _backupMenuPresetDto = new MenuPresetDto
        {
            Id = _menuPresetDto.Id,
            Name = _menuPresetDto.Name,
            Description = _menuPresetDto.Description
        };
    }

    public async Task<bool> Save()
    {
        try
        {
            if (string.IsNullOrEmpty(MenuPresetDto.Name))
            {
                await DialogsHelper.ShowError("Название пресета обязательно");
                return false;
            }
            if (IsNew)
            {
                var presetDto = await HttpClient.Instance.CreatePreset(new CreateMenuPresetRequest
                {
                    Name = MenuPresetDto.Name,
                    Description = MenuPresetDto.Description
                });
                MenuPresetDto.Id = presetDto.Id;
                MenuPresetDto.Name = presetDto.Name;
                MenuPresetDto.Description = presetDto.Description;
            }
            else
            {
                await HttpClient.Instance.UpdatePreset(MenuPresetDto.Id, new UpdateMenuPresetRequest
                {
                    Id = MenuPresetDto.Id,
                    Name = MenuPresetDto.Name,
                    Description = MenuPresetDto.Description
                });
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении пресета: {ex.Message}");
            return false;
        }
    }

    public void Cancel()
    {
        MenuPresetDto.Name = _backupMenuPresetDto.Name;
        MenuPresetDto.Description = _backupMenuPresetDto.Description;
        MenuPresetDto.Id = _backupMenuPresetDto.Id;
    }
}