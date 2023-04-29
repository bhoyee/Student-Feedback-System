using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace API.Services
{
public class Message
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public MailboxAddress From { get; set; }

    public Message(List<MailboxAddress> to, string subject, string body, MailboxAddress from)
    {
        To = to;
        Subject = subject;
        Body = body;
        From = from;
    }

    public MimeMessage CreateMimeMessage()
    {
        var message = new MimeMessage();
        message.From.Add(From);
        message.To.AddRange(To);
        message.Subject = Subject;
        message.Body = new TextPart("plain") { Text = Body };
        return message;
    }
}
}