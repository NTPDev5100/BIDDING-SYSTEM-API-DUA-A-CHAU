using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Gửi mail
    /// </summary>
    public class EmailSendConfigure
    {
        public string SmtpServer { set; get; }
        public IList<string> Tos { get; set; }
        public string EmailTo { get; set; }
        public IList<string> Ccs { get; set; }
        public string From { get; set; }
        public string FromEmail { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string FromDisplayName { get; set; }
        public string Subject { get; set; }
        public MailPriority Priority { get; set; }
        public string ClientCredentialUserName { get; set; }
        public string ClientCredentialPassword { get; set; }
        public IList<string> Bccs { get; set; }

        public EmailSendConfigure()
        {
            Tos = new List<string>();
            Ccs = new List<string>();
            Bccs = new List<string>();
        }
    }
}
