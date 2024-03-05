using Application.Abstractions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Infrastructure.Services.Email;
public class EmailService : IEmailService
{
    private readonly EmailConfig _emailConfig;

    public EmailService(IOptions<EmailConfig> emailConfig)
    {
        _emailConfig = emailConfig.Value;   
    }
    public async Task SendEmailAsync(string receiverEmail,
        string subject,
        string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.SenderEmail));
        emailMessage.To.Add(MailboxAddress.Parse(receiverEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain")
        {
            Text = message
        };
        var smtpClient = new SmtpClient();

        try
        {
            await smtpClient.ConnectAsync(_emailConfig.SmtpServer,
                _emailConfig.SmtpPort,
                useSsl : false);

            await smtpClient.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
            await smtpClient.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
            smtpClient.Dispose();
        }
    }
}