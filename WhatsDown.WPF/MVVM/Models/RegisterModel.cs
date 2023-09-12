using System.Windows.Input;
using System.Windows.Media;

namespace WhatsDown.WPF.MVVM.Models;

class RegisterModel
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Result { get; set; } = "";
    public Color ResultColor { get; set; } = Colors.Black;
    public ICommand SwitchToLoginCmd { get; set; } = null!;
    public ICommand SubmitRegisterCmd { get; set; } = null!;
}