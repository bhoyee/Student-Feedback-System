using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services;

namespace API.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendFeedbackNotificationEmailAsync(string email, string feedbackTitle, int feedbackId);

        Task SendFeedbackNotificationStaffEmailAsync(string email, string feedbackTitle, int feedbackId);
        
        Task SendFeedbackNotificationReplyEmailAsync(string email, string feedbackTitle, int feedbackId);

    }
}