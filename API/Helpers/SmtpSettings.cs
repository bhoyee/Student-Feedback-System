using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class SmtpSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
         public string FromAddress { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }


        // public string Server { get; set; }
        // public int Port { get; set; }
        // public string SenderName { get; set; }
        // public string SenderEmail { get; set; }
        // public string Username { get; set; }
        // public string Password { get; set; }
    }
}