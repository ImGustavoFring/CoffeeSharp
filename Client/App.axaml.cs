using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Client.ViewModels;
using Client.Views;

namespace Client;

public partial class App : Application
{
    public override void Initialize()
    {
        DataTemplates.Add(new StrongViewLocator()
            .Register<LoginViewModel, LoginView>()
            .Register<MainOrdersViewModel, MainOrdersView>()
            .Register<OrderShortItemViewModel, OrderShortItem>()
            .Register<BranchesViewModel, BranchesView>()
            .Register<BranchItemViewModel, BranchItem>()
            .Register<EmployeeRoleViewModel, EmployeeRoleView>()
            .Register<EmployeeRoleItemViewModel, EmployeeRoleItem>()
            .Register<CategoryViewModel, CategoryView>()
            .Register<CategoryItemViewModel, CategoryItem>()
            .Register<ProductViewModel, ProductView>()
            .Register<ProductItemViewModel, ProductItem>()
            // all later are deprecated and will be removed
            .Register<UserContol1VM, UserControl1>()
            .Register<UserControl2VM, UserControl2>());
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}