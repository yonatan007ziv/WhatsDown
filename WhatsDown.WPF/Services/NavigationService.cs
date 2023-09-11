using System;
using WhatsDown.WPF.Core;
using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.Services;

internal class NavigationService : ViewModelBase, INavigationService
{
    private readonly Func<Type, ViewModelBase> _viewModelFactory;
    private ViewModelBase _currentViewModel = null!;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        this._viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        CurrentViewModel = _viewModelFactory.Invoke(typeof(TViewModel));
    }
}