using System.Windows.Input;
using System.Windows.Media;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Core;
using WhatsDown.WPF.Handlers.Networking;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.Models;

namespace WhatsDown.WPF.MVVM.ViewModels;

class LoginViewModel : BaseViewModel, IResultCommunicator<LoginResult>
{
    private readonly ILogger logger;
    private readonly INavigationService navigation;
    private readonly LoginModel model = new LoginModel();

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

    public LoginViewModel(ILogger logger, INavigationService navigation)
    {
        this.logger = logger;
        this.navigation = navigation;

        SwitchToRegisterCmd = new RelayCommand(SwitchToRegister, obj => true);
        SubmitLoginCmd = new RelayCommand(obj => SubmitLogin(), obj => CanSubmitLoginLogic());
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        Result = "";
        ResultColor = Colors.Black;
    }

    private bool CanSubmitLoginLogic()
    {
        return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
    }

    private void SwitchToRegister(object? obj)
    {
        navigation.NavigateTo<RegisterViewModel>();
    }

    private async void SubmitLogin()
    {
        await new LoginRequestPostHandler(logger, this, model).InitiateLoginRequest();
    }

    public void SetResult(LoginResult result)
    {
        switch (result)
        {
            case LoginResult.Success:
                Result = "Sucessfully Logged in";
                ResultColor = Colors.Green;
                break;
            case LoginResult.NoSuchEmail:
                Result = "No Such Email Found";
                ResultColor = Colors.Yellow;
                break;
            case LoginResult.InvalidEmail:
                Result = "Invalid Email";
                ResultColor = Colors.Yellow;
                break;
            case LoginResult.WrongPassword:
                Result = "Wrong Password Entered";
                ResultColor = Colors.Yellow;
                break;
            case LoginResult.InvalidPassword:
                Result = "Invalid Password";
                ResultColor = Colors.Yellow;
                break;
            case LoginResult.ServerUnreachable:
                Result = "Server Unreachable";
                ResultColor = Colors.DarkRed;
                break;
        }
    }
}