namespace WhatsDown.Server.Interfaces.Services;

internal interface ISocketListener
{
	Task StartListening();
}