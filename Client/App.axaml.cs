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
            .Register<EmployeeViewModel, EmployeeView>()
            .Register<EmployeeItemViewModel, EmployeeItem>()
            .Register<AdminViewModel, AdminView>()
            .Register<AdminItemViewModel, AdminItem>()
            .Register<MenuPresetViewModel, MenuPresetView>()
            .Register<MenuPresetItemViewModel, MenuPresetItem>()
            .Register<MenuPresetItemsViewModel, MenuPresetItemsView>()
            .Register<MenuPresetItemsItemViewModel, MenuPresetItemsItem>()
            .Register<BranchMenuItemsViewModel, BranchMenuItemsView>()
            .Register<BranchMenuItemsItemViewModel, BranchMenuItemsItem>());
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
                Title = "Coffee# Client"
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