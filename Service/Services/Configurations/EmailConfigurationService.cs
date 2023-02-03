using Entities;
using Entities.Configuration;
using Entities.DomainEntities;
using Interface.Services.Configuration;
using Interface.UnitOfWork;
using Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Service.Services.DomainServices;
using System.Threading;

namespace Service.Services.Configurations
{
    public class EmailConfigurationService : DomainService<tbl_EmailConfigurations, BaseSearch>, IEmailConfigurationService
    {
        public EmailConfigurationService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <summary>
        /// Lấy thông tin cấu hình Email
        /// </summary>
        /// <returns></returns>
        public async Task<EmailSendConfigure> GetEmailConfig()
        {
            EmailSendConfigure emailSendConfigure = new EmailSendConfigure();
            var configuration = await this.unitOfWork.Repository<tbl_EmailConfigurations>().GetQueryable().Where(e => e.Deleted == false).FirstOrDefaultAsync();
            emailSendConfigure = new EmailSendConfigure()
            {
                Ccs = new string[] { },
                ClientCredentialPassword =configuration.Password,
                ClientCredentialUserName = configuration.userName,
                EnableSsl = configuration.EnableSsl,
                From = configuration.userName,
                FromEmail = configuration.Email,
                FromDisplayName = configuration.DisplayName,
                Port = configuration.Port,
                Priority = MailPriority.Normal,
                SmtpServer = configuration.SmtpServer,
                Subject = string.Empty,
                Tos = new string[] { }
            };
            return emailSendConfigure;
        }

        /// <summary>
        /// Lấy thông tin nội dung email
        /// </summary>
        /// <returns></returns>
        public EmailContent GetEmailContent()
        {
            return new EmailContent()
            {
                isHtml = true,
                content = string.Empty
            };
        }


        public MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)
        {
            MailMessage msg = new MailMessage();
            if (emailConfig.Tos != null)
            {
                foreach (string to in emailConfig.Tos)
                {
                    if (!string.IsNullOrEmpty(to))
                    {
                        msg.To.Add(to);
                    }
                }
            }
            //Chuỗi email
            if (!string.IsNullOrEmpty(emailConfig.EmailTo))
            {
                var emailLists = emailConfig.EmailTo.Split(';');
                if (emailLists != null && emailLists.Any())
                {
                    foreach (var email in emailLists)
                    {
                        if (!string.IsNullOrEmpty(email))
                            msg.To.Add(email);
                    }
                }
            }

            if (emailConfig.Ccs != null)
            {
                foreach (string cc in emailConfig.Ccs)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        msg.CC.Add(cc);
                    }
                }
            }
            if (emailConfig.Bccs != null)

                foreach (string bcc in emailConfig.Bccs)
                {
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        msg.Bcc.Add(bcc);
                    }
                }
            if (string.IsNullOrEmpty(emailConfig.FromEmail))
                emailConfig.FromEmail = emailConfig.From;
            msg.From = new MailAddress(emailConfig.FromEmail,
                                           emailConfig.FromDisplayName,
                                           Encoding.UTF8);
            msg.IsBodyHtml = content.isHtml;
            msg.Body = content.content;
            msg.Priority = emailConfig.Priority;
            msg.Subject = emailConfig.Subject;
            msg.BodyEncoding = Encoding.UTF8;
            msg.SubjectEncoding = Encoding.UTF8;

            if (content.attachments != null && content.attachments.Count > 0)
            {
                foreach (var attachment in content.attachments)
                {
                    msg.Attachments.Add(attachment);
                }
            }
            return msg;
        }

        /// <summary>
        /// Send the email using the SMTP server 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emailConfig"></param>
        public static void  Send(MailMessage message, EmailSendConfigure emailConfig)
        {
            Task.Factory.StartNew(() =>
            {
                SmtpClient client = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                                  emailConfig.ClientCredentialUserName,
                                  emailConfig.ClientCredentialPassword),
                    Host = emailConfig.SmtpServer,
                    Port = emailConfig.Port,
                    EnableSsl = emailConfig.EnableSsl,
                    Timeout = 1000000,
                };
                Console.WriteLine("------------------------------------ Email:" + emailConfig.FromEmail);
                Console.WriteLine("------------------------------------ ClientCredentialUserName:" + emailConfig.ClientCredentialUserName);
                Console.WriteLine("------------------------------------ Password:" + emailConfig.ClientCredentialPassword);
                try
                {
                    //Add this line to bypass the certificate validation
                    //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                    //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    //        System.Security.Cryptography.X509Certificates.X509Chain chain,
                    //        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    //{
                    //    return true;
                    //};
                    client.Send(message);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    Console.WriteLine("SmtpFailedRecipientsExceptionMessage:" + ex.Message);
                    Console.WriteLine("SmtpFailedRecipientsException:" + ex.StackTrace);
                    throw new Exception("SmtpFailedRecipientsException:" + ex.StackTrace);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ExceptionMAILMessage:" + e.Message);
                    Console.WriteLine("ExceptionMAIL:" + e.StackTrace);
                    throw new Exception("ExceptionMAIL:" + e.StackTrace);
                }
                finally
                {
                    message.Dispose();
                }
            });
            
        }

        public async Task Send(string subject, string body, string[] Tos)
        {
            await Send(subject, body, Tos, null, null);
        }


        public async Task Send(string subject, string body, string[] Tos, string[] CCs)
        {
            await Send(subject, body, Tos, CCs, null);
        }


        public async Task Send(string subject, string body, string[] Tos, string[] CCs, string[] BCCs)
        {
            EmailSendConfigure emailConfig = await GetEmailConfig();
            EmailContent content = GetEmailContent();
            emailConfig.Subject = subject;
            emailConfig.Tos = Tos;
            emailConfig.Ccs = CCs;
            emailConfig.Bccs = BCCs;
            content.content = body;
            MailMessage msg = ConstructEmailMessage(emailConfig, content);
            //Thread threadSend = new Thread(delegate() { Send(msg, emailConfig); });
            //threadSend.Start();
            Send(msg, emailConfig);

        }

        public async Task SendMail(string subject, string Tos, string[] CCs, string[] BCCs, EmailContent emailContent)
        {
            EmailSendConfigure emailConfig = await GetEmailConfig();
            EmailContent content = GetEmailContent();
            emailConfig.Subject = subject;
            emailConfig.EmailTo = Tos;
            emailConfig.Ccs = CCs;
            emailConfig.Bccs = BCCs;
            content = emailContent;
            MailMessage msg = ConstructEmailMessage(emailConfig, content);
            Send(msg, emailConfig);
        }

        public async Task Send(string subject, string[] Tos, string[] CCs, string[] BCCs, EmailContent emailContent)
        {
            EmailSendConfigure emailConfig = await GetEmailConfig();
            EmailContent content = GetEmailContent();
            emailConfig.Subject = subject;
            emailConfig.Tos = Tos;
            emailConfig.Ccs = CCs;
            emailConfig.Bccs = BCCs;
            content = emailContent;
            MailMessage msg = ConstructEmailMessage(emailConfig, content);
            Send(msg, emailConfig);
        }
    }
}
