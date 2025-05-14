using System;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.Shared;

namespace Client.ViewModels;

public partial class BranchInformationWindowViewModel(BranchDtoObservable? branchDto, bool isNew) : ViewModelBase
{
    [ObservableProperty] private BranchDtoObservable _branchDto = branchDto ?? new BranchDtoObservable();
    [ObservableProperty] private bool _isNew = isNew;

    private readonly BranchDto _backupBranchDto = new BranchDto()
    {
        Name = branchDto != null ? branchDto.Name: string.Empty,
        Address = branchDto != null ? branchDto.Address: string.Empty,
    };

    public async Task<bool> Save()
    {
        try
        {
            if (IsNew)
            {
                var branchDto = await HttpClient.Instance.CreateBranch(new CreateBranchRequest()
                {
                    Name = BranchDto.Name,
                    Address = BranchDto.Address
                });
                BranchDto.Id = branchDto.Id;
                BranchDto.Name = branchDto.Name;
                BranchDto.Address = branchDto.Address;
            }
            else
            {
                await HttpClient.Instance.UpdateBranch(BranchDto.Id, new UpdateBranchRequest()
                {
                    Id = BranchDto.Id,
                    Name = BranchDto.Name,
                    Address = BranchDto.Address
                });
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(IsNew);
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public void Cancel()
    {
        BranchDto.Name = _backupBranchDto.Name;
        BranchDto.Address = _backupBranchDto.Address;
    }
}