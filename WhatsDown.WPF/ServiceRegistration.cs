using Microsoft.Extensions.DependencyInjection;
using System;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking.Communication;
using WhatsDown.Core.Services.NetworkingEncoders;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.Interfaces.RequestResponse;
using WhatsDown.WPF.MVVM.MVVMCore;
using WhatsDown.WPF.MVVM.MVVMCore.Shell;
using WhatsDown.WPF.MVVM.ViewModels;
using WhatsDown.WPF.Networking.Tcp;
using WhatsDown.WPF.Services;
using WhatsDown.WPF.Services.Logging;
using WhatsDown.WPF.Services.RequestResponseHandlers;

namespace WhatsDown.WPF;

class ServiceRegistration
{
    private readonly IServiceCollection services;

    public ServiceRegistration(IServiceCollection services)
    {
        this.services = services;
    }

    public void AddServices()
    {
        RegisterMainWindow();
        RegisterViewModels();
        RegisterRequiredServices();
        RegisterServices();
    }

    private void RegisterMainWindow()
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainWindowViewModel>()
        });
    }

    private void RegisterViewModels()
    {
        services.AddSingleton<MainMenuViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<RegisterViewModel>();
    }

    private void RegisterRequiredServices()
    {
        // View Model Factory
        services.AddSingleton<Func<Type, BaseViewModel>>(
            provider => viewModelType =>
            (BaseViewModel)provider.GetRequiredService(viewModelType));

        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton<IMessageEncoder<MessagePacket>, JsonMessageEncoder<MessagePacket>>();

        // Login Register Requesters
        services.AddTransient<ILoginRequestResponseHandler, LoginHandlerService>();
        services.AddTransient<IRegisterRequestResponseHandler, RegisterHandlerService>();

        // Network Client
        services.AddTransient<INetworkClient, TcpCommunication>();
    }

    private void RegisterServices()
    {

    }
}