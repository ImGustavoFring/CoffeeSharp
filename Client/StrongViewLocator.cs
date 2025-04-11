using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Client.ViewModels;

namespace Client;

public sealed record ViewDefinition(
    Type ViewType,
    Func<AvaloniaObject> CreateFunc)
{
    /// <summary>
    /// Executes CreateFunc to create a new view.
    /// </summary>
    /// <returns>The newly created view.</returns>
    public object Create() => CreateFunc.Invoke();
}

/// <summary>
/// Strongly-typed View Locator that does not rely on reflection.
/// </summary>
public sealed class StrongViewLocator : IDataTemplate
{
    private readonly Dictionary<Type, ViewDefinition> _views = new();

    public ViewDefinition Locate(object viewModel)
    {
        if (_views.TryGetValue(viewModel.GetType(), out var view))
            return view;

        throw new TypeLoadException($"No view was registered for view model {viewModel.GetType().FullName}.");
    }

    public object Create(object viewModel) => Locate(viewModel).Create();

    public StrongViewLocator Register<TViewModel>(ViewDefinition view)
        where TViewModel : ViewModelBase
    {
        _views[typeof(TViewModel)] = view;
        return this;
    }

    public StrongViewLocator Register<TViewModel, TView>()
        where TViewModel : ViewModelBase
        where TView : AvaloniaObject, new()
    {
        return Register<TViewModel>(new ViewDefinition(typeof(TView), () => new TView()));
    }

    /// <inheritdoc />
    public Control Build(object data)
    {
        try
        {
            return (Control)Create(data!);
        }
        catch
        {
            return new TextBlock
            {
                Text = $"No view registered for {data?.GetType().FullName}"
            };
        }
    }

    /// <inheritdoc />
    public bool Match(object data) => data is ViewModelBase;
}