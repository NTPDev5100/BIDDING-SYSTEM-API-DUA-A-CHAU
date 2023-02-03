using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Utilities
{
    public class EmailContent
    {
        public EmailContent()
        {
            attachments = new List<Attachment>();
        }

        public bool isHtml { get; set; }
        public string content { get; set; }
        public IList<Attachment> attachments { get; set; }
    }
}
