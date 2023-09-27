using System.Collections.Generic;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.ViewModels.Elements;

namespace WhatsDown.WPF.MVVM.Models.Pages;

internal class ChatsUnifiedModel
{
	public IEnumerable<ChatModel> ChatList { get; set; } = null!;
	public ChatsScrollViewModel ChatsScrollView { get; set; } = null!;
	public IChatNavigation ChatNavigation { get; set; } = null!;
}