using MimeKit.Cryptography;

namespace Infrastructure.Services.Email;
public class EmailConfig
{
    
    public string SenderEmail { get; set; } = string.Empty;
    public string SmtpServer {  get; set; } = string.Empty; 
    public int SmtpPort { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
