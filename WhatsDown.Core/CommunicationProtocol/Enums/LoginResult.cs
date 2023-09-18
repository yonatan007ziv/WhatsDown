namespace WhatsDown.Core.CommunicationProtocol.Enums;

public enum LoginResult
{
    Success,
    NoSuchEmailExists,
    InvalidEmail,
    WrongPassword,
    InvalidPassword,
    ServerUnreachable,
    UnknownError,
}