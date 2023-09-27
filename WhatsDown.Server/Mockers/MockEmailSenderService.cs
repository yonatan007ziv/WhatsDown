using WhatsDown.Core.Interfaces;
using WhatsDown.Server.Interfaces.Services;

namespace WhatsDown.Server.Mockers;

internal class MockEmailSenderService : IEmailSender
{
	private readonly ILogger logger;

	public MockEmailSenderService(ILogger logger)
	{
		this.logger = logger;
	}

	public Task<bool> SendMessage(string target, string title, string content)
	{
		logger.LogInformation($"Mock IEmailSender.SendMessage: Title:({title}), Content:({content})");
		return Task.FromResult(true);
	}
}