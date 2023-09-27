using System.Collections.ObjectModel;
using WhatsDown.WPF.MVVM.ViewModels.Elements;

namespace WhatsDown.WPF.MVVM.Models.Elements;

class ChatsScrollModel
{
	public ObservableCollection<ChatButtonViewModel> ChatList { get; set; } = null!;
}