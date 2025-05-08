using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;

namespace Client.ObservableDTO;

public partial class BranchDtoObservable : ObservableObject
{
    [ObservableProperty]
    private long _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _address = string.Empty;

    public BranchDtoObservable() { }

    public BranchDtoObservable(BranchDto dto)
    {
        _id = dto.Id;
        _name = dto.Name;
        _address = dto.Address;
    }

    public BranchDto ToDto()
    {
        return new BranchDto
        {
            Id = Id,
            Name = Name,
            Address = Address
        };
    }
}