using System.Drawing;
using System.Windows.Input;
using WhatsDown.Core.Models;

namespace WhatsDown.WPF.MVVM.Models.Elements;

internal class ChatButtonModel
{
	public string ChatName { get; set; } = null!;
	public Bitmap ChatImage { get; internal set; } = null!;
	public ChatModel ChatModel { get; internal set; } = null!;
	public ICommand ClickedChatCmd { get; set; } = null!;
}