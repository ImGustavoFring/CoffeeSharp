using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.ObservableDTO;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class BranchesView : UserControl
{
    public BranchesView()
    {
        InitializeComponent();
        SearchBranches(null, null);
    }
    
    private async void SearchBranches(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BranchesViewModel vm)
        {
            var searchNameQuery = vm.SearchNameQuery;
            var searchAddressQuery = vm.SearchAddressQuery;

            try
            {
                var rsp = await HttpClient.Instance.GetAllBranches(searchNameQuery, searchAddressQuery);
                var branches = rsp.branches;
                vm.Branches.Clear();
                foreach (var branch in branches)
                {
                    var bvm = new BranchItemViewModel(new BranchDtoObservable(branch));         
                    vm.Branches.Add(bvm);
                }
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    private async void CreateBranch(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is Window ww)
        {
            var dc = new BranchInformationWindowViewModel(null, true);
            var window = new BranchInformationWindow()
            {
                DataContext = dc
            };
            await window.ShowDialog(ww);
            if (dc.BranchDto.Id != 0)
            {
                if (DataContext is BranchesViewModel vm)
                {
                    vm.Branches.Add(new BranchItemViewModel(dc.BranchDto));
                }
            }
        }
    }
}