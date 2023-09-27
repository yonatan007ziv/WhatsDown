using System.Windows.Input;
using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.MVVM.MVVMCore.Shell;

internal class MainWindowModel
{
	public IPageNavigation Navigation { get; set; } = null!;
	public ICommand DragWindowCmd { get; set; } = null!;
	public ICommand MinimizeCmd { get; set; } = null!;
	public ICommand MaximizeCmd { get; set; } = null!;
	public ICommand CloseCmd { get; set; } = null!;
}