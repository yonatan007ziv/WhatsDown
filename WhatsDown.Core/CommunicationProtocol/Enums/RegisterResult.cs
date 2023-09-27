namespace WhatsDown.Core.CommunicationProtocol.Enums;

public enum RegisterResult
{
	UnknownError, // Default
	TwoFANeeded,
	EmailExists,
	InvalidEmail,
	InvalidPassword,
	InvalidCredentials,
	ServerUnreachable,
	Timeout,
}