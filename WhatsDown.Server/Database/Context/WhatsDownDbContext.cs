using Microsoft.EntityFrameworkCore;
using WhatsDown.Server.Database.Models;

namespace WhatsDown.Server.Database.Context;

public class WhatsDownDbContext : DbContext
{
    #region Table definitions
    public DbSet<AccountCredentials> AccountCredentials { get; set; }
    public DbSet<UserInformation> UserInformation { get; set; }
    #endregion

    public WhatsDownDbContext(DbContextOptions<WhatsDownDbContext> options)
        : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WhatsDownDB;Integrated Security=True;");
        base.OnConfiguring(optionsBuilder);
    }
}