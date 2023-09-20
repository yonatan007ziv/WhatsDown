using WhatsDown.WPF.MVVM.MVVMCore;

namespace WhatsDown.WPF.Interfaces;

interface INavigationController
{
    BaseViewModel CurrentViewModel { get; }
    void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
}