using System;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.MVVMCore;

namespace WhatsDown.WPF.DI.Services.Navigation;

internal class PageNavigationService : BaseElementViewModel, IPageNavigation
{
	private readonly Func<Type, BasePageViewModel> pageFactory;
	private BasePageViewModel _currentPage = null!;

	public BasePageViewModel CurrentPage
	{
		get => _currentPage;
		set
		{
			_currentPage = value;
			OnPropertyChanged();
		}
	}

	public PageNavigationService(Func<Type, BasePageViewModel> pageFactory)
	{
		this.pageFactory = pageFactory;
	}

	public void NavigateTo<TViewModel>() where TViewModel : BasePageViewModel
	{
		CurrentPage?.Exit();
		CurrentPage = pageFactory(typeof(TViewModel));
		CurrentPage.Enter();
	}
}