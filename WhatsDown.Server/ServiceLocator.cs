using Microsoft.Extensions.DependencyInjection;
using WhatsDown.Core.Interfaces;

namespace WhatsDown.Server;

public class ServiceLocator
{
    private static IServiceProvider _serviceProvider = null!;
    public static IServiceProvider ServiceProvider
    {
        get => _serviceProvider;
        set
        {
            if (ServiceProvider != null)
            {
                ILogger logger = ServiceProvider.GetRequiredService<ILogger>();
                logger.LogWarning("ServiceProvider is already set!");
            }
            else
                _serviceProvider = value;
        }
    }
}