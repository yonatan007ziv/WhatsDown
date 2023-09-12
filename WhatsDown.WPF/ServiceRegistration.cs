using Microsoft.Extensions.DependencyInjection;
using System;
using WhatsDown.Core.Interfaces;
using WhatsDown.WPF.Core;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.Shell;
using WhatsDown.WPF.MVVM.ViewModels;
using WhatsDown.WPF.Services;
using WhatsDown.WPF.Services.Logging;

namespace WhatsDown.WPF;

class ServiceRegistration
{
    private readonly IServiceCollection services;

    public ServiceRegistration(IServiceCollection services)
    {
        this.services = services;

        // Main Window
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainWindowViewModel>()
        });
    }

    public void AddServices()
    {
        RegisterViewModels();
        RegisterRequiredServices();
        RegisterServices();
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
        services.AddSingleton<ILogger, MessageBoxLogging>();
    }

    private void RegisterServices()
    {
        
    }
}