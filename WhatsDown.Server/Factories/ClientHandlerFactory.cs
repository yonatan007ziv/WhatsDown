using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using WhatsDown.Core.Interfaces;
using WhatsDown.Server.Handlers;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Interfaces.Services.Database;
using WhatsDown.Server.Interfaces.Services.Factories;
using WhatsDown.Server.Networking;

namespace WhatsDown.Server.Factories;

internal class ClientHandlerFactory : IFactory<TcpClient, IClientHandler>
{
	private readonly IServiceProvider provider;
	private readonly ILogger logger;

	public ClientHandlerFactory(IServiceProvider provider, ILogger logger)
	{
		this.provider = provider;
		this.logger = logger;
	}

	public IClientHandler Create(TcpClient socket)
	{
		return new ClientHandler(
			new TcpCommunication(
					socket,
					provider.GetRequiredService<ISerializer>(),
					provider.GetRequiredService<IConfigurationFetcher>(),
					logger
					),
				provider.GetRequiredService<IDatabaseAnalyzer>(),
				provider.GetRequiredService<IEmailSender>(),
				provider.GetRequiredService<ITokenGenerator<string>>(),
				provider.GetRequiredService<ISerializer>(),
				logger);
	}
}