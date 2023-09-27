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

class LoginViewModel : BasePageViewModel
{
	private readonly IPageNavigation navigation;
	private readonly ILoginRequest loginHandler;
	private readonly IResourceExtractor resourceExtractor;
	private readonly LoginModel model = new LoginModel();

	private CancellationTokenSource updateTimeoutBarCts = new CancellationTokenSource();
	private Task<LoginResult>? currentProcedure;

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
	public string ResultText
	{
		get => model.ResultText;
		set
		{
			model.ResultText = value;
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
	public ICommand SwitchToRegisterCmd
	{
		get => model.SwitchToRegisterCmd;
		set
		{
			model.SwitchToRegisterCmd = value;
			OnPropertyChanged();
		}
	}
	public ICommand SubmitLoginCmd
	{
		get => model.SubmitLoginCmd;
		set
		{
			model.SubmitLoginCmd = value;
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

	public LoginViewModel(IPageNavigation navigation, ILoginRequest loginHandler, IResourceExtractor resourceExtractor)
	{
		this.navigation = navigation;
		this.loginHandler = loginHandler;
		this.resourceExtractor = resourceExtractor;

		SwitchToRegisterCmd = new RelayCommand(SwitchToRegister, obj => true);
		SubmitLoginCmd = new RelayCommand(obj => SubmitLogin(), obj => CanSubmitLoginLogic());
	}

	public override void Enter()
	{
		updateTimeoutBarCts = new CancellationTokenSource();
		ResultText = "";
		ResultColor = Colors.Black;
		UpdateTimeoutBar();
	}

	public override void Exit()
	{
		updateTimeoutBarCts.Cancel();
		loginHandler.StopProcedure();
	}

	private async void UpdateTimeoutBar()
	{
		while (!updateTimeoutBarCts.IsCancellationRequested)
		{
			TimeoutPercentage = loginHandler.GetTimeoutPercentage();
			await Task.Delay(10);
		}
	}

	private bool CanSubmitLoginLogic()
	{
		return !string.IsNullOrEmpty(Email)
			&& !string.IsNullOrEmpty(Password)
			&& (currentProcedure == null || currentProcedure.IsCompleted);
	}

	private void SwitchToRegister(object? obj)
	{
		navigation.NavigateTo<RegisterViewModel>();
	}

	private async void SubmitLogin()
	{
		if (currentProcedure != null && !currentProcedure.IsCompleted)
			return;
		ResultText = "";

		CommandManager.InvalidateRequerySuggested();

		currentProcedure = loginHandler.Procedure(new CredentialsModel { Email = model.Email, Password = model.Password });
		LoginResult result = await currentProcedure;
		SetResult(result);

		CommandManager.InvalidateRequerySuggested();
	}

	public void SetResult(LoginResult result)
	{
		if (result == LoginResult.Success)
			navigation.NavigateTo<ChatsUnifiedViewModel>();

		ResultText = resourceExtractor.GetString($"Result_Login_{result}_Message");
		ResultColor = resourceExtractor.GetColor($"Result_Login_{result}_Color");
	}
}