using WhatsDown.WPF.Core;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.ViewModels;

namespace WhatsDown.WPF.MVVM.Shell;

class MainWindowViewModel : BaseViewModel
{
    private INavigationService _navigation;

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        _navigation.NavigateTo<LoginViewModel>();
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}