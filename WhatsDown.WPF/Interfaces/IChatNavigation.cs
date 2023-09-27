using WhatsDown.Core.Models;

namespace WhatsDown.WPF.Interfaces;

internal interface IChatNavigation
{
	void NavigateTo(ChatModel chat);
}