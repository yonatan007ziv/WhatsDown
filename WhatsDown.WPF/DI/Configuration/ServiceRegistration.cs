using Microsoft.Extensions.DependencyInjection;
using System;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Services;
using WhatsDown.WPF.DI.Services;
using WhatsDown.WPF.DI.Services.Configuration;
using WhatsDown.WPF.DI.Services.Logging;
using WhatsDown.WPF.DI.Services.RequestResponseServices;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.Interfaces.Aliases;
using WhatsDown.WPF.Interfaces.AppConfiguration;
using WhatsDown.WPF.MVVM.MVVMCore;
using WhatsDown.WPF.MVVM.MVVMCore.Shell;
using WhatsDown.WPF.MVVM.ViewModels;
using WhatsDown.WPF.Networking.Tcp;
using WhatsDown.WPF.Utils;

namespace WhatsDown.WPF.DI.Configuration;

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
		RegisterOptionalServices();
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
		services.AddSingleton<ChatScrollerViewModel>();
	}

	private void RegisterRequiredServices()
	{
		// View Model Factory
		services.AddSingleton<Func<Type, BaseViewModel>>(
			provider => viewModelType =>
			(BaseViewModel)provider.GetRequiredService(viewModelType));

		services.AddSingleton<INavigationController, NavigationService>();
		services.AddSingleton<ILogger, ConsoleLogger>();
		services.AddScoped<ISerializer<MessagePacket>, JsonMessageSerializerService>();
		services.AddScoped<IChatUserMessageSerializer, JsonChatUserMessageSerializerService>();

		// Login Register Requesters
		services.AddTransient<ILoginRequest, LoginRequestService>();
		services.AddTransient<IRegisterRequest, RegisterRequestService>();

		// Network Client
		services.AddSingleton<INetworkClient, TcpCommunication>();

		// Configuration
		services.AddSingleton<IApplicationConfigurationFileManager, ApplicationConfigurationFileManagerService>();
		services.AddSingleton<ISerializer<AppSettingsModel>, JsonAppConfigurationSerializerService>();

		// Resource Extractor
		services.AddSingleton<IResourceExtractor, ResourceExtractorService>();

		// Settings
		services.AddSingleton<ISettingsManager, SettingsManagerService>();
	}

	private void RegisterOptionalServices()
	{

	}
}