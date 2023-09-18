using System.ComponentModel;
using System.Runtime.CompilerServices;
using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.MVVM.MVVMCore;

public abstract class BaseViewModel : INotifyPropertyChanged, IViewTransition
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string? propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public abstract void Enter();
    public abstract void Exit();
}