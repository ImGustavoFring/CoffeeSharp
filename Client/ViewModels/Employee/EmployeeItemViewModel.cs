using System;
using System.Linq;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

public partial class EmployeeItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private EmployeeDtoObservable _employeeDto;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasBranch))]
    private string? _branchName;

    [ObservableProperty]
    private string? _roleName;

    public bool HasBranch => !string.IsNullOrEmpty(BranchName);

    public EmployeeItemViewModel(EmployeeDtoObservable employeeDto)
    {
        _employeeDto = employeeDto;
        LoadDetailsAsync();
    }

    private async void LoadDetailsAsync()
    {
        try
        {
            if (EmployeeDto.RoleId.HasValue)
            {
                var role = EmployeeRoleViewModel.Instance.EmployeeRoles.FirstOrDefault(r => r.EmployeeRoleDto.Id == EmployeeDto.RoleId.Value);
                if (role == null)
                {
                    var rsp = await HttpClient.Instance.GetAllEmployeeRoles();
                    EmployeeRoleViewModel.Instance.EmployeeRoles.Clear();
                    foreach (var r in rsp.Items)
                    {
                        EmployeeRoleViewModel.Instance.EmployeeRoles.Add(new EmployeeRoleItemViewModel(new EmployeeRoleDtoObservable(r)));
                    }
                    role = EmployeeRoleViewModel.Instance.EmployeeRoles.FirstOrDefault(r => r.EmployeeRoleDto.Id == EmployeeDto.RoleId.Value);
                }
                RoleName = role?.EmployeeRoleDto.Name ?? "Неизвестна";
            }

            // Загрузка имени филиала
            if (EmployeeDto.BranchId.HasValue)
            {
                var branch = BranchesViewModel.Instance.Branches.FirstOrDefault(b => b.BranchDto.Id == EmployeeDto.BranchId.Value);
                if (branch == null)
                {
                    var rsp = await HttpClient.Instance.GetAllBranches();
                    BranchesViewModel.Instance.Branches.Clear();
                    foreach (var b in rsp.branches)
                    {
                        BranchesViewModel.Instance.Branches.Add(new BranchItemViewModel(new BranchDtoObservable(b)));
                    }
                    branch = BranchesViewModel.Instance.Branches.FirstOrDefault(b => b.BranchDto.Id == EmployeeDto.BranchId.Value);
                }
                BranchName = branch?.BranchDto.Name;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке данных для EmployeeId={EmployeeDto.Id}: {ex.Message}");
        }
    }
}