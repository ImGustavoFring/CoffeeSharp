using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Services;
using Client.ViewModels;

namespace Client.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AuthService.Instance.StartUpLogin();
        RefreshAuthData();
    }

    private void RefreshAuthData()
    {
        if (DataContext is MainWindowViewModel vm)
        {
            AuthService.Instance.MainWindowViewModel = vm;
            vm.RefreshAuthData();
        }
    }
}