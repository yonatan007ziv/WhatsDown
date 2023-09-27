using System.Windows.Input;
using WhatsDown.Core.Models;
using WhatsDown.WPF.MVVM.Models.Elements;
using WhatsDown.WPF.MVVM.MVVMCore;

namespace WhatsDown.WPF.MVVM.ViewModels.Elements;

internal class ChatButtonViewModel : BaseElementViewModel
{
	private readonly ChatButtonModel model;

	public ChatModel ChatModel
	{
		get => model.ChatModel;
		set
		{
			model.ChatModel = value;
			OnPropertyChanged();
		}
	}
	public ICommand ClickedChatCmd
	{
		get => model.ClickedChatCmd;
		set
		{
			model.ClickedChatCmd = value;
			OnPropertyChanged();
		}
	}

	public ChatButtonViewModel()
	{
		model = new ChatButtonModel();
	}
}