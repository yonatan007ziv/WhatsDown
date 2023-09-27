using System.Windows.Input;
using System.Windows.Media;

namespace WhatsDown.WPF.MVVM.Models.Pages;

public class LoginModel
{
	public string Email { get; set; } = "";
	public string Password { get; set; } = "";
	public string ResultText { get; set; } = "";
	public Color ResultColor { get; set; } = Colors.Black;
	public ICommand SwitchToRegisterCmd { get; set; } = null!;
	public ICommand SubmitLoginCmd { get; set; } = null!;
	public int TimeoutPercentage { get; set; }
}