using Entities.Configuration;
using Entities.DomainEntities;
using Interface.Services.DomainServices;
using Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services.Configuration
{
    public interface IEmailConfigurationService : IDomainService<tbl_EmailConfigurations, BaseSearch>
    {
        Task<EmailSendConfigure> GetEmailConfig();
        EmailContent GetEmailContent();
        Task Send(string subject, string body, string[] Tos);
        Task Send(string subject, string body, string[] Tos, string[] CCs);
        Task Send(string subject, string body, string[] Tos, string[] CCs, string[] BCCs);
        Task Send(string subject, string[] Tos, string[] CCs, string[] BCCs, EmailContent emailContent);
        Task SendMail(string subject, string Tos, string[] CCs, string[] BCCs, EmailContent emailContent);
    }
}
