using System;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;
using Domain.DTOs.User.Requests;

namespace Client.ViewModels;

public partial class AdminInformationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private AdminDtoObservable _adminDto;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private string? _password;

    private readonly AdminDto _backupAdminDto;

    public AdminInformationWindowViewModel(AdminDtoObservable? adminDto, bool isNew)
    {
        _adminDto = adminDto ?? new AdminDtoObservable();
        _isNew = isNew;
        _backupAdminDto = new AdminDto
        {
            Id = _adminDto.Id,
            UserName = _adminDto.UserName
        };
    }

    public async Task<bool> Save()
    {
        try
        {
            if (IsNew)
            {
                if (string.IsNullOrEmpty(Password))
                {
                    await DialogsHelper.ShowError("Пароль обязателен при создании администратора");
                    return false;
                }
                var adminDto = await HttpClient.Instance.AddAdmin(new CreateAdminRequest(AdminDto.UserName, Password));
                AdminDto.Id = adminDto.Id;
                AdminDto.UserName = adminDto.UserName;
            }
            else
            {
                await HttpClient.Instance.UpdateAdmin(AdminDto.Id, new UpdateAdminRequest(AdminDto.Id, AdminDto.UserName, Password));
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении администратора: {ex.Message}");
            return false;
        }
    }

    public void Cancel()
    {
        AdminDto.UserName = _backupAdminDto.UserName;
        AdminDto.Id = _backupAdminDto.Id;
        Password = null;
    }
}