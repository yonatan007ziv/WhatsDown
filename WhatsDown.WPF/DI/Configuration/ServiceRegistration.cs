using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;
using WhatsDown.Core.Services;
using WhatsDown.WPF.DI.Services.Configuration;
using WhatsDown.WPF.DI.Services.Logging;
using WhatsDown.WPF.DI.Services.Navigation;
using WhatsDown.WPF.DI.Services.RequestResponseServices;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.Interfaces.Aliases;
using WhatsDown.WPF.Interfaces.AppConfiguration;
using WhatsDown.WPF.Mocking;
using WhatsDown.WPF.MVVM.MVVMCore;
using WhatsDown.WPF.MVVM.MVVMCore.Shell;
using WhatsDown.WPF.MVVM.ViewModels.Elements;
using WhatsDown.WPF.MVVM.ViewModels.Pages;
using WhatsDown.WPF.Networking.Tcp;

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
		// Pages
		services.AddSingleton<MainMenuViewModel>();
		services.AddSingleton<LoginViewModel>();
		services.AddSingleton<RegisterViewModel>();
		services.AddSingleton<ChatsUnifiedViewModel>();

		// Elements
		services.AddSingleton<ChatButtonViewModel>();
		services.AddSingleton<ChatExplorerViewModel>();
		services.AddSingleton<ChatsScrollViewModel>();

		#region Navigation Services
		// Page Factory
		services.AddSingleton<Func<Type, BasePageViewModel>>(
			provider => viewModelType =>
			(BasePageViewModel)provider.GetRequiredService(viewModelType));
		services.AddSingleton<IPageNavigation, PageNavigationService>();

		// Chat Explorer Factory
		services.AddSingleton<Func<ChatModel, ChatExplorerViewModel>>(
			provider => chatModel =>
			{
				ChatExplorerViewModel chatExplorer = provider.GetRequiredService<ChatExplorerViewModel>();
				chatExplorer.ChatModel = chatModel;
				return chatExplorer;
			});
		services.AddSingleton<IChatNavigation, ChatNavigationService>();
		#endregion
	}

	private void RegisterRequiredServices()
	{
		// Configuration
		IConfigurationBuilder configBuilder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");
		IConfiguration configuration = configBuilder.Build();
		services.AddSingleton(configuration);
		services.AddSingleton<IConfigurationFetcher, ConfigurationFetcherService>();

		services.AddSingleton<ILogger, ConsoleLogger>();

		// Serialization
		services.AddScoped<ISerializer, JsonSerializerService>();
		services.AddScoped<IMessageSerializer, MessageSerializerService>();

		// Login Register Requesters
		// services.AddScoped<ILoginRequest, MockLoginRequestService>();
		services.AddScoped<ILoginRequest, LoginRequestService>();
		services.AddScoped<IRegisterRequest, RegisterRequestService>();

		// Validated User Requests
		services.AddScoped<IValidatedUserRequest, MockValidatedUserRequestService>();
		// services.AddScoped<IValidatedUserRequest, ValidatedUserRequestService>();

		// Network Client
		services.AddSingleton<INetworkClient, TcpCommunication>();

		// Configuration
		services.AddSingleton<IApplicationConfigurationFileManager, ApplicationConfigurationFileManagerService>();

		// Resource Extractor
		services.AddSingleton<IResourceExtractor, ResourceExtractorService>();

		// Settings
		services.AddSingleton<ISettingsManager, SettingsManagerService>();
	}

	private void RegisterOptionalServices()
	{

	}
}