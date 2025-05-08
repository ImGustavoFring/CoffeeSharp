using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class AdminDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _userName = string.Empty;

    public AdminDtoObservable() { }

    public AdminDtoObservable(AdminDto dto)
    {
        _id = dto.Id;
        _userName = dto.UserName;
    }

    public AdminDto ToDto()
    {
        return new AdminDto
        {
            Id = Id,
            UserName = UserName
        };
    }
}