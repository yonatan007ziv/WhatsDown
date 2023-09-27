using WhatsDown.Core.Models;
using WhatsDown.WPF.MVVM.Models.Elements;
using WhatsDown.WPF.MVVM.MVVMCore;

namespace WhatsDown.WPF.MVVM.ViewModels.Elements;

class ChatExplorerViewModel : BaseElementViewModel
{
	private readonly ChatExplorerModel model;

	public ChatModel ChatModel
	{
		get => model.ChatModel;
		set
		{
			model.ChatModel = value;
			OnPropertyChanged();
		}
	}

	public ChatExplorerViewModel()
	{
		model = new ChatExplorerModel();
	}
}