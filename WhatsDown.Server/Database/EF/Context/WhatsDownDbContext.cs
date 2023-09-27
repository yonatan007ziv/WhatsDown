using Microsoft.EntityFrameworkCore;
using WhatsDown.Server.Database.OrmModels;

namespace WhatsDown.Server.Database.EF.Context;

internal class WhatsDownDbContext : DbContext
{
	#region Table definitions
	public DbSet<Credentials_DbModel> AccountCredentials { get; set; }
	public DbSet<Users_DbModel> UserInformation { get; set; }
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