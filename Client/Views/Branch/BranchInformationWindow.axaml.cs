using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client.Utils;
using Client.ViewModels;
using Domain.DTOs.Shared;

namespace Client.Views;

public partial class BranchInformationWindow : Window
{
    public BranchInformationWindow()
    {
        InitializeComponent();
    }

    private async void Save(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BranchInformationWindowViewModel vm)
        {
            if (await vm.Save())
            {
                await DialogsHelper.ShowOk();
                Close();
            }
            else
            {
                await DialogsHelper.ShowError();
            }
        }
    }

    public void Cancel(object? sender, RoutedEventArgs routedEventArgs)
    {
        if (DataContext is BranchInformationWindowViewModel vm)
        {
            vm.Cancel();
            Close();
        }
    }
}