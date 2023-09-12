using WhatsDown.WPF.Core;

namespace WhatsDown.WPF.Interfaces;

interface INavigationService
{
    BaseViewModel CurrentViewModel { get; }
    void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
}