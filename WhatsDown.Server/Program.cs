using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Services;
using WhatsDown.Server.Factories;
using WhatsDown.Server.Handlers;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Interfaces.Services.Database;
using WhatsDown.Server.Interfaces.Services.Factories;
using WhatsDown.Server.Interfaces.Services.Security;
using WhatsDown.Server.Services;
using WhatsDown.Server.Services.Database.SqlDirect;
using WhatsDown.Server.Services.Hashing;

namespace WhatsDown.Server;

public class Program
{
    public static async Task Main()
    {
        var services = new ServiceCollection();
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);

        IConfiguration configuration = configBuilder.Build();
        services.AddSingleton(configuration);
        services.AddSingleton<IConfigurationFetcher, ConfigurationFetcherService>();

		#region Database
		#region IAnalyzer & IExtractor Decoupling
        /*
		services.AddSingleton<IDatabaseAnalyzer, DatabaseAnalyzer>();
		// EF Core
        services.AddDbContext<WhatsDownDbContext>(
            options =>
            options.UseSqlServer(configuration.GetSection("Database:Sql").GetConnectionString("Default"))
        );
        services.AddSingleton<IDatabaseExtractor, EfDbExtractor>();

        // Dapper
        // ...
        */
		#endregion
		//services.AddSingleton<IDatabaseHandler, SqlDbHandler>(); // Sql
		services.AddSingleton<IDatabaseHandler, SqLiteDbHandler>(); // SqLite
		#endregion

		// Factories
		services.AddSingleton<IFactory<TcpClient, IClientHandler>, ClientHandlerFactory>(
            provider =>
            new ClientHandlerFactory(provider, provider.GetRequiredService<ILogger>())
        );

        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton<ISocketListener, TcpListenerHandler>();
        services.AddScoped<IEmailSender, SmtpService>();
        services.AddScoped<ISerializer<MessagePacket>, JsonMessageSerializerService>();
		services.AddScoped<IChatUserMessageSerializer, JsonChatUserMessageSerializerService>();

		// Token Generator
		services.AddSingleton<ITokenGenerator<string>, StringTokenGenerator>();

		// Hashing Salting
		services.AddSingleton<IHasher, Md5HashingService>();
		services.AddSingleton<ISalter, Md5SaltingService>();

		IServiceProvider provider = services.BuildServiceProvider();
        await ServerLoop(provider);
    }

    public static async Task ServerLoop(IServiceProvider provider)
    {
        try
        {
            ISocketListener server = provider.GetRequiredService<ISocketListener>();
            await server.StartListening();
        }
        catch (Exception ex)
        {
            provider.GetRequiredService<ILogger>()
                .LogFatal($"Server Crashed: {ex.Message}");
        }
    }
}