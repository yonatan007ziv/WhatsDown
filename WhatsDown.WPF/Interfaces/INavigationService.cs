using WhatsDown.WPF.Core;

namespace WhatsDown.WPF.Interfaces;

interface INavigationService
{
    ViewModelBase CurrentViewModel { get; }
    void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
}