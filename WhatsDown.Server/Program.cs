using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Interfaces.Networking;
using WhatsDown.Core.Interfaces.Networking.Communication;
using WhatsDown.Core.Services.NetworkingEncoders;
using WhatsDown.Server.Database.Context;
using WhatsDown.Server.Factories;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Interfaces.Services.Database;
using WhatsDown.Server.Interfaces.Services.Factories;
using WhatsDown.Server.Networking;
using WhatsDown.Server.Services;
using WhatsDown.Server.Services.Database;
using WhatsDown.Server.Services.Networking;

namespace WhatsDown.Server;

public class Program
{
    private static IServiceProvider provider;

    public static async Task Main()
    {
        var services = new ServiceCollection();
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);

        IConfiguration configuration = builder.Build();

        // Database
        services.AddDbContext<WhatsDownDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetSection("Database").GetConnectionString("SqlDb"));
        });
        services.AddTransient<IDatabaseExtractor, EFDbExtractor>();
        services.AddTransient<IDatabaseAnalyzer, DatabaseAnalyzer>();

        // Factories
        // INetworkCommunication factory
        services.AddTransient<Func<TcpClient, INetworkCommunication>>(provider =>
            tcpClient =>
            {
                return new TcpCommunication(
                    tcpClient,
                    provider.GetRequiredService<IMessageEncoder<MessagePacket>>(),
                    provider.GetRequiredService<ILogger>()
                    );
            }
        );
        services.AddSingleton<IClientCommunicationFactory<TcpClient>, ClientCommunicationFactory<TcpClient>>();

        services.AddSingleton(configuration);
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton<ISocketListener, TcpLoopListenerService>();
        services.AddSingleton<IConfigurationFetcher, ConfigurationFetcherService>();
        services.AddTransient<IEmailSender, SmtpService>();
        services.AddScoped<IMessageEncoder<MessagePacket>, JsonMessageEncoder<MessagePacket>>();

        provider = services.BuildServiceProvider();

        await ServerLoop();
    }

    public static async Task ServerLoop()
    {
        try
        {
            await provider.GetRequiredService<ISocketListener>().StartListening();
        }
        catch (Exception ex)
        {
            provider.GetRequiredService<ILogger>()
                .LogFatal($"Server Crashed: {ex.Message}");
        }
    }
}