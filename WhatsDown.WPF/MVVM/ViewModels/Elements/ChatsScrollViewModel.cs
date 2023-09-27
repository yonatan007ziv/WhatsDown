using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.Models.Elements;
using WhatsDown.WPF.MVVM.MVVMCore;

namespace WhatsDown.WPF.MVVM.ViewModels.Elements;

class ChatsScrollViewModel : BaseElementViewModel
{
	private readonly IChatNavigation chatNavigation;
	private readonly ILogger logger;
	private readonly ChatsScrollModel model;

	public IEnumerable<ChatModel> Chats
	{
		set
		{
			ChatButtons = new ObservableCollection<ChatButtonViewModel>(value.Select(element => new ChatButtonViewModel { ChatModel = element }));
		}
	}
	public ObservableCollection<ChatButtonViewModel> ChatButtons
	{
		get => model.ChatList;
		set
		{
			model.ChatList = value;
			OnPropertyChanged();

			foreach (ChatButtonViewModel c in value)
				c.ClickedChatCmd = new RelayCommand(chat => OnClickChat((ChatModel)chat!), obj => true);
		}
	}

	public ChatsScrollViewModel(IChatNavigation chatNavigation, ILogger logger)
	{
		this.chatNavigation = chatNavigation;
		this.logger = logger;

		model = new ChatsScrollModel();
	}

	private void OnClickChat(ChatModel chat)
	{
		chatNavigation.NavigateTo(chat);
	}
}