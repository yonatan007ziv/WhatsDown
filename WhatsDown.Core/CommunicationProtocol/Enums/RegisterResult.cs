namespace WhatsDown.Core.CommunicationProtocol.Enums;

public enum RegisterResult
{
    UnknownError, // Default
    Success,
    EmailExists,
    InvalidEmail,
    InvalidPassword,
    InvalidCredentials,
    ServerUnreachable,
    Timeout,
}