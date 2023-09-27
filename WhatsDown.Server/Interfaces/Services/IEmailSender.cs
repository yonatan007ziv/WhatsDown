namespace WhatsDown.Server.Interfaces.Services;

internal interface IEmailSender
{
	Task<bool> SendMessage(string target, string title, string content);
}