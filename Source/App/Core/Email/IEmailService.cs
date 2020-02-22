using System;
using System.Threading.Tasks;

namespace Project.Core.Email
{
    public interface IEmailService
    {
        Task SendConfirmationEmailToUser(string awaitingVerifyEmail, string toDisplayName, string confirmationCode, DateTime? exireTime);
    }
}