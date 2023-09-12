using System;
using WhatsDown.WPF.Core;
using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.Services;

internal class NavigationService : BaseViewModel, INavigationService
{
    private readonly Func<Type, BaseViewModel> viewModelFactory;
    private BaseViewModel _currentViewModel = null!;

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public NavigationService(Func<Type, BaseViewModel> viewModelFactory)
    {
        this.viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
    {
        CurrentViewModel?.Exit();
        CurrentViewModel = viewModelFactory.Invoke(typeof(TViewModel));
        CurrentViewModel.Enter();
    }

    public override void Enter()
    {
        throw new NotImplementedException();
    }

    public override void Exit()
    {
        throw new NotImplementedException();
    }
}