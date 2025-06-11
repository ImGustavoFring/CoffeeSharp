using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs.Shared;
using Domain.DTOs.User.Requests;

namespace Client.ViewModels;

public partial class EmployeeInformationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private EmployeeDtoObservable _employeeDto;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private ObservableCollection<BranchDtoObservable> _availableBranches;

    [ObservableProperty]
    private ObservableCollection<EmployeeRoleDtoObservable> _availableRoles;

    [ObservableProperty]
    private BranchDtoObservable? _selectedBranch;

    [ObservableProperty]
    private EmployeeRoleDtoObservable? _selectedRole;

    private readonly EmployeeDto _backupEmployeeDto;

    public EmployeeInformationWindowViewModel(EmployeeDtoObservable? employeeDto, bool isNew)
    {
        _employeeDto = employeeDto ?? new EmployeeDtoObservable();
        _isNew = isNew;
        _backupEmployeeDto = new EmployeeDto
        {
            Name = _employeeDto.Name,
            UserName = _employeeDto.UserName,
            RoleId = _employeeDto.RoleId,
            BranchId = _employeeDto.BranchId
        };

        // Инициализация филиалов
        _availableBranches = new ObservableCollection<BranchDtoObservable>
        {
            new BranchDtoObservable { Id = 0, Name = "Без филиала" }
        };
        LoadBranchesAsync();

        // Инициализация ролей
        _availableRoles = new ObservableCollection<EmployeeRoleDtoObservable>();
        LoadRolesAsync();
    }

    private async void LoadBranchesAsync()
    {
        try
        {
            if (!BranchesViewModel.Instance.Branches.Any())
            {
                var rsp = await HttpClient.Instance.GetAllBranches();
                BranchesViewModel.Instance.Branches.Clear();
                foreach (var branch in rsp.branches)
                {
                    BranchesViewModel.Instance.Branches.Add(new BranchItemViewModel(new BranchDtoObservable(branch)));
                }
            }
            foreach (var branch in BranchesViewModel.Instance.Branches)
            {
                AvailableBranches.Add(branch.BranchDto);
            }
            if (EmployeeDto.BranchId.HasValue)
            {
                SelectedBranch = AvailableBranches.FirstOrDefault(b => b.Id == EmployeeDto.BranchId);
            }
            else
            {
                SelectedBranch = AvailableBranches.FirstOrDefault(b => b.Id == 0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке филиалов: {ex.Message}");
        }
    }

    private async void LoadRolesAsync()
    {
        try
        {
            if (!EmployeeRoleViewModel.Instance.EmployeeRoles.Any())
            {
                var rsp = await HttpClient.Instance.GetAllEmployeeRoles();
                EmployeeRoleViewModel.Instance.EmployeeRoles.Clear();
                foreach (var role in rsp.Items)
                {
                    EmployeeRoleViewModel.Instance.EmployeeRoles.Add(new EmployeeRoleItemViewModel(new EmployeeRoleDtoObservable(role)));
                }
            }
            foreach (var role in EmployeeRoleViewModel.Instance.EmployeeRoles)
            {
                AvailableRoles.Add(role.EmployeeRoleDto);
            }
            if (_employeeDto.RoleId.HasValue)
            {
                SelectedRole = AvailableRoles.FirstOrDefault(r => r.Id == _employeeDto.RoleId);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке ролей: {ex.Message}");
        }
    }

    partial void OnSelectedRoleChanged(EmployeeRoleDtoObservable? value)
    {
        EmployeeDto.RoleId = value?.Id;
    }

    partial void OnSelectedBranchChanged(BranchDtoObservable? value)
    {
        EmployeeDto.BranchId = value?.Id == 0 ? null : value?.Id;
    }

    public async Task<bool> Save()
    {
        try
        {
            if (IsNew)
            {
                if (string.IsNullOrEmpty(Password))
                {
                    await DialogsHelper.ShowError("Пароль обязателен при создании сотрудника");
                    return false;
                }
                var employeeDto = await HttpClient.Instance.AddEmployee(new CreateEmployeeRequest
                {
                    Name = EmployeeDto.Name,
                    UserName = EmployeeDto.UserName,
                    Password = Password,
                    RoleId = EmployeeDto.RoleId!.Value,
                    BranchId = EmployeeDto.BranchId
                });
                EmployeeDto.Id = employeeDto.Id;
                EmployeeDto.Name = employeeDto.Name;
                EmployeeDto.UserName = employeeDto.UserName;
                EmployeeDto.RoleId = employeeDto.RoleId;
                EmployeeDto.BranchId = employeeDto.BranchId;
            }
            else
            {
                await HttpClient.Instance.UpdateEmployee(EmployeeDto.Id, new UpdateEmployeeRequest
                {
                    Id = EmployeeDto.Id,
                    Name = EmployeeDto.Name,
                    UserName = EmployeeDto.UserName,
                    Password = Password,
                    RoleId = EmployeeDto.RoleId!.Value,
                    BranchId = EmployeeDto.BranchId
                });
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении сотрудника: {ex.Message}");
            return false;
        }
    }

    public void Cancel()
    {
        EmployeeDto.Name = _backupEmployeeDto.Name;
        EmployeeDto.UserName = _backupEmployeeDto.UserName;
        EmployeeDto.RoleId = _backupEmployeeDto.RoleId;
        EmployeeDto.BranchId = _backupEmployeeDto.BranchId;
        SelectedRole = AvailableRoles.FirstOrDefault(r => r.Id == _backupEmployeeDto.RoleId);
        SelectedBranch = AvailableBranches.FirstOrDefault(b => b.Id == (_backupEmployeeDto.BranchId ?? 0));
        Password = null;
    }
}