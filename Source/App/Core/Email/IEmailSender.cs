using System.Threading.Tasks;

namespace Project.Core.Email
{
    public interface IEmailSender
    {
        Task SendSecurityEmailAsync(string to, string toDisplayName, string subject, string body);
        Task SendEmailAsync(string from, string to, string toDisplayName, string subject, string body);
        Task SendEmailAsync(string from, string fromDisplayName, string to, string toDisplayName, string subject,
            string body);
    }
}