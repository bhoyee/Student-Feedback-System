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
using API.Entities;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendnewEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Student Feedback", "s.feedback@outlook.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };
            

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("s.feedback@outlook.com", "StuFeed0#");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
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
                await client.AuthenticateAsync("s.feedback@outlook.com", "StuFeed0#");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        public async Task SendFeedbackNotificationEmailAsync(string email, string feedbackTitle, int feedbackId)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Student Feedback", "s.feedback@outlook.com"));
            message.To.Add(new MailboxAddress("",email));
            message.Subject = "New Feedback Available";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Dear student,</p><p>A new feedback with title '{feedbackTitle}' is available for you to view. Click <a href='https://sfbapi.azurewebsites.net/api/feedbacks/{feedbackId}'>here</a> to view the feedback.</p>";
            bodyBuilder.TextBody = $"Dear student, a new feedback with title '{feedbackTitle}' is available for you to view. Click here: https://sfbapi.azurewebsites.net/api/feedbacks/{feedbackId}";

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("s.feedback@outlook.com", "StuFeed0#");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        public async Task SendFeedbackNotificationStaffEmailAsync(string email, string feedbackTitle, int feedbackId)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Student Feedback", "s.feedback@outlook.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "New Feedback Available";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Dear staff,</p><p>A new feedback with title '{feedbackTitle}' is available for you to view. Click <a href='https://sfbapi.azurewebsites.net/api/feedbacks/{feedbackId}'>here</a> to view the feedback.</p>";
            bodyBuilder.TextBody = $"Dear staff, a new feedback with title '{feedbackTitle}' is available for you to view. Click here: https://sfbapi.azurewebsites.net/api/feedbacks/{feedbackId}";

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("s.feedback@outlook.com", "StuFeed0#");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendFeedbackNotificationReplyEmailAsync(string email, string feedbackTitle, int feedbackId)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Student Feedback", "s.feedback@outlook.com"));
            message.To.Add(new MailboxAddress("",email));
            message.Subject = "New feedback reply";

            var bodyBuilder = new BodyBuilder();
            
            bodyBuilder.TextBody = $"Dear staff, a new feedback with title '{feedbackTitle}' is available for you to view. Click here: https://sfbapi.azurewebsites.net/api/feedbacks/{feedbackId}";
            bodyBuilder.TextBody = $"A new reply has been added to your feedback ({feedbackTitle}).\n\n" +
                       $"You can view the feedback and its replies here: https://sfbapi.azurewebsites.net/api/feedbacks/{feedbackId}";

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("s.feedback@outlook.com", "StuFeed0#");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

}