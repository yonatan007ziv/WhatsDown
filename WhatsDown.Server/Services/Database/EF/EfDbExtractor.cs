using WhatsDown.Server.Database.EF.Context;

namespace WhatsDown.Server.Services.Database.EF;

// future planning, decreases coupling
internal class EfDbExtractor // : IDatabaseExtractor prevent unnecessary compilation errors
{
	private readonly WhatsDownDbContext context;

	public EfDbExtractor(WhatsDownDbContext context)
	{
		this.context = context;
	}
}
