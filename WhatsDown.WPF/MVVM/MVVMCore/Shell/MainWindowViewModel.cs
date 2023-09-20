using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.ViewModels;

namespace WhatsDown.WPF.MVVM.MVVMCore.Shell;

class MainWindowViewModel : BaseViewModel
{
    private INavigationController _navigation;

    public INavigationController Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel(INavigationController navigation, ILogger logger)
    {
        _navigation = navigation;
        _navigation.NavigateTo<LoginViewModel>();

        logger.LogInformation("Started WPF Debug");
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