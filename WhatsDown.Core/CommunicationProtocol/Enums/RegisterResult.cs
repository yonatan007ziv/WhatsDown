namespace WhatsDown.Core.CommunicationProtocol.Enums;

public enum RegisterResult
{
    Success,
    EmailExists,
    InvalidEmail,
    InvalidPassword,
    ServerUnreachable
}