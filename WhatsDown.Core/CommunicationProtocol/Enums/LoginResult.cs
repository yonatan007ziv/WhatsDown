namespace WhatsDown.Core.CommunicationProtocol.Enums;

public enum LoginResult
{
    UnknownError, // Default
    Success,
    NoSuchEmailExists,
    InvalidEmail,
    WrongPassword,
    InvalidPassword,
    InvalidCredentials,
    ServerUnreachable,
    Timeout,
}