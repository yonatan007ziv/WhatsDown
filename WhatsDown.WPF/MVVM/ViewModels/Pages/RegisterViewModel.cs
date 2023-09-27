using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.Interfaces.Aliases;
using WhatsDown.WPF.Interfaces.AppConfiguration;
using WhatsDown.WPF.MVVM.Models.Pages;
using WhatsDown.WPF.MVVM.MVVMCore;

namespace WhatsDown.WPF.MVVM.ViewModels.Pages;

class RegisterViewModel : BasePageViewModel
{
	private readonly IPageNavigation navigation;
	private readonly IRegisterRequest registerHandler;
	private readonly IResourceExtractor resourceExtractor;
	private readonly RegisterModel model = new RegisterModel();

	private CancellationTokenSource updateTimeoutBarCts = new CancellationTokenSource();
	private Task<RegisterResult>? currentProcedure;

	public string Email
	{
		get => model.Email;
		set
		{
			model.Email = value;
			OnPropertyChanged();
			CommandManager.InvalidateRequerySuggested();
		}
	}
	public string Password
	{
		get => model.Password;
		set
		{
			model.Password = value;
			OnPropertyChanged();
			CommandManager.InvalidateRequerySuggested();
		}
	}
	public string Result
	{
		get => model.Result;
		set
		{
			model.Result = value;
			OnPropertyChanged();
		}
	}
	public Color ResultColor
	{
		get => model.ResultColor;
		set
		{
			model.ResultColor = value;
			OnPropertyChanged();
		}
	}
	public ICommand SwitchToLoginCmd
	{
		get => model.SwitchToLoginCmd;
		set
		{
			model.SwitchToLoginCmd = value;
			OnPropertyChanged();
		}
	}
	public ICommand SubmitRegisterCmd
	{
		get => model.SubmitRegisterCmd;
		set
		{
			model.SubmitRegisterCmd = value;
			OnPropertyChanged();
		}
	}
	public int TimeoutPercentage
	{
		get => model.TimeoutPercentage;
		set
		{
			model.TimeoutPercentage = value;
			OnPropertyChanged();
		}
	}

	public RegisterViewModel(IPageNavigation navigation, IRegisterRequest registerHandler, IResourceExtractor resourceExtractor)
	{
		this.navigation = navigation;
		this.registerHandler = registerHandler;
		this.resourceExtractor = resourceExtractor;

		SwitchToLoginCmd = new RelayCommand(SwitchToLogin, obj => true);
		SubmitRegisterCmd = new RelayCommand(obj => SubmitRegister(), obj => CanSubmitRegisterLogic());
	}

	public override void Enter()
	{
		updateTimeoutBarCts = new CancellationTokenSource();
		Result = "";
		ResultColor = Colors.Black;
		UpdateTimeoutBar();
	}

	public override void Exit()
	{
		updateTimeoutBarCts.Cancel();
		registerHandler.StopProcedure();
	}

	private async void UpdateTimeoutBar()
	{
		while (!updateTimeoutBarCts.IsCancellationRequested)
		{
			TimeoutPercentage = registerHandler.GetTimeoutPercentage();
			await Task.Delay(10);
		}
	}

	private bool CanSubmitRegisterLogic()
	{
		return !string.IsNullOrEmpty(Email)
			&& !string.IsNullOrEmpty(Password)
			&& (currentProcedure == null || currentProcedure.IsCompleted);
	}

	private void SwitchToLogin(object? obj)
	{
		navigation.NavigateTo<LoginViewModel>();
	}

	private async void SubmitRegister()
	{
		if (currentProcedure != null && !currentProcedure.IsCompleted)
			return;
		Result = "";

		CommandManager.InvalidateRequerySuggested();

		currentProcedure = registerHandler.Procedure(new CredentialsModel { Email = model.Email, Password = model.Password });
		RegisterResult result = await currentProcedure;
		SetResult(result);

		CommandManager.InvalidateRequerySuggested();
	}

	public void SetResult(RegisterResult result)
	{
		Result = resourceExtractor.GetString($"Result_Register_{result}_Message");
		ResultColor = resourceExtractor.GetColor($"Result_Register_{result}_Color");
	}
}