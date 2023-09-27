using WhatsDown.Server.Interfaces.Services.Database;

namespace WhatsDown.Server.Services.Database;

internal class DatabaseAnalyzer // : IDatabaseAnalyzer prevent unnecessary compilation errors
{
	private readonly IDatabaseExtractor extractor;

	public DatabaseAnalyzer(IDatabaseExtractor extractor)
	{
		this.extractor = extractor;
	}
}