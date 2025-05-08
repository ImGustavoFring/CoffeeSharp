using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class EmployeeDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private long? _roleId;

    [ObservableProperty]
    private long? _branchId;

    public EmployeeDtoObservable() { }

    public EmployeeDtoObservable(EmployeeDto dto)
    {
        _id = dto.Id;
        _name = dto.Name;
        _userName = dto.UserName;
        _roleId = dto.RoleId;
        _branchId = dto.BranchId;
    }

    public EmployeeDto ToDto()
    {
        return new EmployeeDto
        {
            Id = Id,
            Name = Name,
            UserName = UserName,
            RoleId = RoleId,
            BranchId = BranchId
        };
    }
}