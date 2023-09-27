using WhatsDown.WPF.MVVM.MVVMCore;

namespace WhatsDown.WPF.Interfaces;

interface IPageNavigation
{
	void NavigateTo<TViewModel>() where TViewModel : BasePageViewModel;
}