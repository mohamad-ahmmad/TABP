namespace Application.Abstractions;
public interface IEmailService
{
    Task SendEmailAsync(string receiverEmail,
        string subject,
        string message
        );
}
