using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.Models.Pages;
using WhatsDown.WPF.MVVM.MVVMCore;
using WhatsDown.WPF.MVVM.ViewModels.Elements;

namespace WhatsDown.WPF.MVVM.ViewModels.Pages;

class ChatsUnifiedViewModel : BasePageViewModel
{
	private readonly IValidatedUserRequest requestHandler;
	private readonly ILogger logger;
	private readonly ChatsUnifiedModel model;

	public IChatNavigation ChatNavigation
	{
		get => model.ChatNavigation;
		set
		{
			model.ChatNavigation = value;
			OnPropertyChanged();
		}
	}
	public ChatsScrollViewModel ChatsScrollView
	{
		get => model.ChatsScrollView;
		set
		{
			model.ChatsScrollView = value;
			OnPropertyChanged();
		}
	}

	public ChatsUnifiedViewModel(IValidatedUserRequest requestHandler, IChatNavigation chatNavigation, ILogger logger,
		ChatsScrollViewModel chatsScrollViewModel)
	{
		this.requestHandler = requestHandler;
		this.logger = logger;

		model = new ChatsUnifiedModel();

		ChatNavigation = chatNavigation;
		ChatsScrollView = chatsScrollViewModel;
	}

	public override async void Enter()
	{
		ChatsScrollView.Chats = await requestHandler.GetChats();
	}

	public override void Exit()
	{

	}
}