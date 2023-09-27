namespace WhatsDown.Server.Interfaces.Services;

internal interface ITokenGenerator<T>
{
	T GenerateUniqueToken();
}