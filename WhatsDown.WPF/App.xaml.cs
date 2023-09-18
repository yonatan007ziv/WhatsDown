using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WhatsDown.WPF.MVVM.MVVMCore.Shell;

namespace WhatsDown.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        IServiceCollection services = new ServiceCollection();

        new ServiceRegistration(services).AddServices();
        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _serviceProvider.GetRequiredService<MainWindow>().Show();
        base.OnStartup(e);
    }
}