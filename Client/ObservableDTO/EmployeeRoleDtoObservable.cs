using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class EmployeeRoleDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _name = string.Empty;

    public EmployeeRoleDtoObservable() { }

    public EmployeeRoleDtoObservable(EmployeeRoleDto dto)
    {
        _id = dto.Id;
        _name = dto.Name;
    }

    public EmployeeRoleDto ToDto()
    {
        return new EmployeeRoleDto
        {
            Id = Id,
            Name = Name
        };
    }
}