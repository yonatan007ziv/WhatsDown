using Microsoft.EntityFrameworkCore;
using WhatsDown.Core.Interfaces;
using WhatsDown.Server.Interfaces.Services;

namespace WhatsDown.Server.Database.Context;

internal class WhatsDownDbContext : DbContext
{
    private readonly ILogger logger;
    private readonly IConfigurationService configuration;

    public WhatsDownDbContext(ILogger logger, IConfigurationService configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetStringAttribute("ConnectionStrings:SqlDb"));
    }
}