using System.Net;
using System.Net.Mail;
using WhatsDown.Core.Interfaces;
using WhatsDown.Server.Exceptions;
using WhatsDown.Server.Interfaces.Services;

namespace WhatsDown.Server.Services;

internal class SmtpService : IEmailSendingService
{
    private readonly SmtpClient smtpClient = null!;
    private readonly MailMessage message = null!;

    private readonly string smtpEmail, smtpPassword;
    private readonly int port;
    private readonly ILogger logger;

    public SmtpService(IConfigurationService configuration, ILogger logger)
    {
        try
        {
            smtpClient = new SmtpClient();
            message = new MailMessage();

            smtpEmail = configuration.GetStringAttribute("Smtp:Email");
            smtpPassword = configuration.GetStringAttribute("Smtp:Password");
            port = configuration.GetIntAttribute("Smtp:Port");
        }
        catch (ConfigurationAttributeNotFound ex) 
        {
            logger.LogError($"Error Initializing SmtpService: {ex.Message}");
            throw new ServiceInitializationException(nameof(SmtpService));
        }
        catch (Exception ex)
        {
            logger.LogError($"Error Initializing SmtpService: {ex.Message}");
            throw new ServiceInitializationException(nameof(SmtpService));
        }

        this.logger = logger;
    }

    public async Task<bool> SendMessage(string target, string title, string content)
    {
        try
        {
            smtpClient.Port = port;
            smtpClient.Credentials = new NetworkCredential(smtpEmail, smtpPassword);
            smtpClient.EnableSsl = true;

            message.From = new MailAddress(smtpEmail);
            message.To.Add(target);
            message.Subject = title;
            message.Body = content;

            await smtpClient.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error Sending Email: {ex.Message}");
        }
        return false;
    }

    public void Dispose()
    {
        smtpClient.Dispose();
        message.Dispose();
    }
}