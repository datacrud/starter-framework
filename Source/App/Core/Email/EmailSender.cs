using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using Project.Core.Email;
using Project.Core.Extensions;
using Serilog;
using Typesafe.Mailgun;

namespace Project.Core.EmailPublisher
{
    public class EmailSender : IEmailSender
    {
        public bool EnableEmailFeature { get; }
        private string NoReplySender { get; }
        private string MailGunApiBaseUrl { get; }
        private string MailGunDomain { get; }
        private string MailGunApiKey { get; }
        

        public EmailSender()
        {
            EnableEmailFeature = bool.Parse(ConfigurationManager.AppSettings["Emailing:IsEnabled"]);
            NoReplySender = ConfigurationManager.AppSettings["Emailing:NoReplySender"];

            MailGunApiBaseUrl = ConfigurationManager.AppSettings["Emailing:MailGunApiBaseUrl"];
            MailGunDomain = ConfigurationManager.AppSettings["Emailing:MailGunDomain"];
            MailGunApiKey = ConfigurationManager.AppSettings["EMailing:MailGunApiKey"];
        }
        

        public async Task SendSecurityEmailAsync(string to, string toDisplayName, string subject, string body)
        {            
            try
            {
                if (!EnableEmailFeature) return;

                await Task.Delay(0);

                var from = NoReplySender;
                const string fromDisplayName = "Data Crud";

                var message = new MailMessage(new MailAddress(from, fromDisplayName), new MailAddress(to, toDisplayName))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,                    
                };

                var client = new MailgunClient(MailGunDomain, MailGunApiKey, 3);
                client.SendMail(message);
            }
            catch (Exception e)
            {
                Log.Information(e.ToString());
            }
        }

        public async Task SendEmailAsync(string from, string to, string toDisplayName, string subject, string body)
        {
            try
            {
                if (!EnableEmailFeature) return;

                await Task.Delay(0);

                if(from.IsNullEmptyOrWhiteSpace()) from = NoReplySender;
                const string fromDisplayName = "Data Crud";

                if (toDisplayName.IsNullEmptyOrWhiteSpace()) toDisplayName = to;
                
                var message = new MailMessage(new MailAddress(from, fromDisplayName), new MailAddress(to, toDisplayName))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };


                var client = new MailgunClient(MailGunDomain, MailGunApiKey, 3);
                client.SendMail(message);

                //var message = new MailMessage(NoReplySender, to, subject, body);
                //or
                //client.SendMail(new MailMessage(NoReplySender, to)
                //{
                //    Subject = subject,
                //    Body = body
                //});
            }
            catch (Exception e)
            {
                Log.Information(e.ToString());
            }
        }

        public async Task SendEmailAsync(string from, string fromDisplayName, string to, string toDisplayName, string subject, string body)
        {
            try
            {
                if (!EnableEmailFeature) return;

                await Task.Delay(0);

                if (from.IsNullEmptyOrWhiteSpace())
                {
                    from = NoReplySender;
                    fromDisplayName = "Data Crud";
                }

                if (fromDisplayName.IsNullEmptyOrWhiteSpace()) fromDisplayName = "Data Crud";
                if (toDisplayName.IsNullEmptyOrWhiteSpace()) toDisplayName = to;

                var message = new MailMessage(new MailAddress(from, fromDisplayName),
                    new MailAddress(to, toDisplayName))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                var client = new MailgunClient(MailGunDomain, MailGunApiKey, 3);
                client.SendMail(message);               
            }
            catch (Exception e)
            {
                Log.Information(e.ToString());
            }
        }
    }
}
