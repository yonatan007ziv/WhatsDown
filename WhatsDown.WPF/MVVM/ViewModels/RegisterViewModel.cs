using System.Windows.Input;
using System.Windows.Media;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Core;
using WhatsDown.WPF.Handlers.Networking;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.Models;

namespace WhatsDown.WPF.MVVM.ViewModels;

class RegisterViewModel : BaseViewModel, IResultCommunicator<RegisterResult>
{
    private readonly ILogger logger;
    private readonly INavigationService navigation;
    private readonly RegisterModel model = new RegisterModel();

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

    public RegisterViewModel(ILogger logger, INavigationService navigation)
    {
        this.logger = logger;
        this.navigation = navigation;

        SwitchToLoginCmd = new RelayCommand(SwitchToLogin, obj => true);
        SubmitRegisterCmd = new RelayCommand(obj => SubmitRegister(), obj => CanSubmitRegisterLogic());
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {
        Result = "";
        ResultColor = Colors.Black;
    }

    private bool CanSubmitRegisterLogic()
    {
        return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
    }

    private void SwitchToLogin(object? obj)
    {
        navigation.NavigateTo<LoginViewModel>();
    }

    private async void SubmitRegister()
    {
        await new RegisterRequestPostHandler(logger, this, model).InitiateRegisterRequest();
    }

    public void SetResult(RegisterResult result)
    {
        switch (result)
        {
            case RegisterResult.Success:
                Result = "Successfully registered, 2FA";
                ResultColor = Colors.Green;
                break;
            case RegisterResult.EmailExists:
                Result = "Email Already in Use";
                ResultColor = Colors.Yellow;
                break;
            case RegisterResult.InvalidEmail:
                Result = "Invalid Email";
                ResultColor = Colors.Yellow;
                break;
            case RegisterResult.InvalidPassword:
                Result = "Invalid Password";
                ResultColor = Colors.Yellow;
                break;
            case RegisterResult.ServerUnreachable:
                Result = "Server Unreachable";
                ResultColor = Colors.DarkRed;
                break;
        }
    }
}