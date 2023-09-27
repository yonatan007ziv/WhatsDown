using System.Windows;
using System.Windows.Input;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.ViewModels.Pages;

namespace WhatsDown.WPF.MVVM.MVVMCore.Shell;

class MainWindowViewModel : BasePageViewModel
{
	private readonly MainWindowModel model;

	public IPageNavigation Navigation
	{
		get => model.Navigation;
		set
		{
			model.Navigation = value;
			OnPropertyChanged();
		}
	}
	public ICommand DragWindowCmd
	{
		get => model.DragWindowCmd;
		set
		{
			model.DragWindowCmd = value;
			OnPropertyChanged();
		}
	}
	public ICommand MinimizeCmd
	{
		get => model.MinimizeCmd;
		set
		{
			model.MinimizeCmd = value;
			OnPropertyChanged();
		}
	}
	public ICommand MaximizeCmd
	{
		get => model.MaximizeCmd;
		set
		{
			model.MaximizeCmd = value;
			OnPropertyChanged();
		}
	}
	public ICommand CloseCmd
	{
		get => model.CloseCmd;
		set
		{
			model.CloseCmd = value;
			OnPropertyChanged();
		}
	}

	public MainWindowViewModel(IPageNavigation navigation)
	{
		model = new MainWindowModel();

		Navigation = navigation;
		DragWindowCmd = new RelayCommand(obj => DragWindow(), obj => true);
		MinimizeCmd = new RelayCommand(obj => MinimizeWindow(), obj => true);
		MaximizeCmd = new RelayCommand(obj => MaximizeWindow(), obj => true);
		CloseCmd = new RelayCommand(obj => CloseWindow(), obj => true);

		Navigation.NavigateTo<LoginViewModel>();
	}


	private void DragWindow()
	{
		if (Mouse.LeftButton == MouseButtonState.Pressed)
			Application.Current.MainWindow.DragMove();
	}

	private void MinimizeWindow()
	{
		Application.Current.MainWindow.WindowState = WindowState.Minimized;
	}

	private bool maximized;
	private void MaximizeWindow()
	{
		Application.Current.MainWindow.WindowState = maximized ? WindowState.Normal : WindowState.Maximized;
		maximized = !maximized;
	}

	private void CloseWindow()
	{
		Application.Current.Shutdown();
	}

	public override void Enter() { }

	public override void Exit() { }
}