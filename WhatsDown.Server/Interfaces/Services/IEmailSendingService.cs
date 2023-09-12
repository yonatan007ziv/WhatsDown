namespace WhatsDown.Server.Interfaces.Services;

internal interface IEmailSendingService
{
    Task<bool> SendMessage(string target, string title, string content);
}