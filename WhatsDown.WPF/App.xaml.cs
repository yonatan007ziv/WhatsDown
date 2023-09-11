using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WhatsDown.WPF.Core;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.Shell;
using WhatsDown.WPF.MVVM.ViewModels;
using WhatsDown.WPF.Services;

namespace WhatsDown.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    IServiceProvider _serviceProvider;

    public App()
    {
        IServiceCollection services = new ServiceCollection();

        // View Model Factory
        services.AddSingleton<Func<Type, ViewModelBase>>(
            provider =>
            viewModelType =>
            (ViewModelBase)provider.GetRequiredService(viewModelType));
        // Navigation Service
        services.AddSingleton<INavigationService, NavigationService>();
        // Main Window
        services.AddSingleton(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainWindowViewModel>()
        });

        // View Models Registration
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainMenuViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<RegisterViewModel>();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _serviceProvider.GetRequiredService<MainWindow>().Show();
    }
}