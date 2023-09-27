using System;
using System.Windows.Input;

namespace WhatsDown.WPF.MVVM.MVVMCore;

public class RelayCommand : ICommand
{
	private readonly Action<object?> execute;
	private readonly Predicate<object?> predicate;

	public event EventHandler? CanExecuteChanged
	{
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}

	public RelayCommand(Action<object?> execute, Predicate<object?> predicate)
	{
		this.execute = execute;
		this.predicate = predicate;
	}

	public bool CanExecute(object? parameter)
	{
		return predicate(parameter);
	}

	public void Execute(object? parameter)
	{
		execute(parameter);
	}
}