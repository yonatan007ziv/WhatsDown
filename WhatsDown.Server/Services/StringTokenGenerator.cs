using WhatsDown.Server.Interfaces.Services;

namespace WhatsDown.Server.Services;

internal class StringTokenGenerator : ITokenGenerator<string>
{
	private List<string> _tokens = new List<string>();

	public string GenerateUniqueToken()
	{
		int stringLength = 10;
		string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*)(_+-=";
		char[] stringChars = new char[stringLength];
		Random random = new Random();

		for (int i = 0; i < stringLength; i++)
			stringChars[i] = chars[random.Next(chars.Length)];

		string final = new string(stringChars);
		if (_tokens.Contains(final))
			return GenerateUniqueToken();

		_tokens.Add(final);
		return final;
	}
}