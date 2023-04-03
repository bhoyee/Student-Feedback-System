using System;
using System.Net.Mail;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using API.Helpers;

namespace API.Services
{
   public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Student Feedback", "s.feedback@outlook.com"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            await client.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("s.feedback@outlook.com", "Rain1234#");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}

}