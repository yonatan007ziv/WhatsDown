using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatsDown.Server.Interfaces.Services;
using WhatsDown.Server.Networking.Clients.Servers;
using WhatsDown.Server.Services;

namespace WhatsDown.Server;

public class Program
{
    public static async Task Main()
    {
        var services = new ServiceCollection();
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);

        IConfiguration configuration = builder.Build();

        services.AddSingleton(configuration);
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddTransient<IEmailSender, SmtpService>();

        ServiceLocator.ServiceProvider = services.BuildServiceProvider();

        try
        {
            await new TcpServer().Start();
        }
        catch (Exception ex)
        {
            ServiceLocator.ServiceProvider.GetRequiredService<ILogger>()
                .LogFatal($"Server Crashed: {ex.Message}");
        }
    }
}