using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace IdentityService.Services;
public class EmailSender(IConfiguration configuration, ILogger<EmailSender> logger) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress(configuration["EmailSettings:SenderName"], configuration["EmailSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            await client.ConnectAsync(configuration["EmailSettings:SmtpServer"], 
                int.Parse(configuration["EmailSettings:Port"]!), 
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(configuration["EmailSettings:Username"], configuration["EmailSettings:Password"]);
            
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
            logger.LogInformation($"Email sent successfully to {email}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error sending email to {email}");
            throw;
        }
    }
}