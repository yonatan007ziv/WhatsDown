using System;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.MVVMCore;
using WhatsDown.WPF.MVVM.ViewModels.Elements;

namespace WhatsDown.WPF.DI.Services.Navigation;

internal class ChatNavigationService : BaseElementViewModel, IChatNavigation
{
	private readonly Func<ChatModel, ChatExplorerViewModel> chatExplorerFactory;
	private ChatExplorerViewModel _currentChat = null!;

	public ChatExplorerViewModel CurrentChat
	{
		get => _currentChat;
		set
		{
			_currentChat = value;
			OnPropertyChanged();
		}
	}

	public ChatNavigationService(Func<ChatModel, ChatExplorerViewModel> chatExplorerFactory)
	{
		this.chatExplorerFactory = chatExplorerFactory;
	}

	public void NavigateTo(ChatModel chat)
	{
		CurrentChat = chatExplorerFactory(chat);
	}
}