using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.MVVM.MVVMCore;

internal abstract class BasePageViewModel : BaseElementViewModel, IViewTransition
{
	public abstract void Enter();
	public abstract void Exit();
}