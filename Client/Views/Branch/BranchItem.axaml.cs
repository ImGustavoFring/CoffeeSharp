using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

namespace Client.Views;

public partial class BranchItem : UserControl
{
    public BranchItem()
    {
        InitializeComponent();
    }

    private void OpenInformationWindow(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BranchItemViewModel vm && this.VisualRoot is Window ww)
        {
            var window = new BranchInformationWindow()
            {
                DataContext = new BranchInformationWindowViewModel(vm.BranchDto, false)
            };
            window.ShowDialog(ww);
        }
    }

    private async void DeleteBranch(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BranchItemViewModel vm)
        {
            try
            {
                await HttpClient.Instance.DeleteBranch(vm.BranchDto.Id);
                await DialogsHelper.ShowOk();
                BranchesViewModel.Instance.Branches.Remove(vm);
            }
            catch
            {
                await DialogsHelper.ShowError();
            }
        }
    }
}