using WhatsDown.Server.Interfaces.Services.Security;

namespace WhatsDown.Server.Services.Hashing;

internal class Md5SaltingService : ISalter
{
	public string GenerateSalt()
	{
		throw new NotImplementedException();
	}
}