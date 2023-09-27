using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WhatsDown.WPF.MVVM.MVVMCore;

internal abstract class BaseElementViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public void OnPropertyChanged([CallerMemberName] string? propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}