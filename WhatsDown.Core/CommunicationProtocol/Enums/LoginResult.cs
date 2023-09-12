namespace WhatsDown.Core.CommunicationProtocol.Enums;

public enum LoginResult
{
    Success,
    NoSuchEmail,
    InvalidEmail,
    WrongPassword,
    InvalidPassword,
    ServerUnreachable
}