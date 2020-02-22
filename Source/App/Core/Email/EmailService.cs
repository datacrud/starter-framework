using System;
using System.Threading.Tasks;

namespace Project.Core.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendConfirmationEmailToUser(string awaitingVerifyEmail, string toDisplayName, string confirmationCode, DateTime? expireTime)
        {
            const string subject = "Confirm Your Email";
            var body =
                $"This email has been send to you to verify your email <strong> {awaitingVerifyEmail} </strong>. Please use the following verification code to confirm your email.<br/><br/> <strong>Code:</strong/> {confirmationCode} <br/><br/> This code will be valid for next 30 minutes only";

            if (expireTime.HasValue) body += $" and will expire on {expireTime: dd/MM/yyyy hh:mm tt}. ";
            else body += ". ";

            body += $"Please use the code before expire.";

            await _emailSender.SendSecurityEmailAsync(awaitingVerifyEmail, toDisplayName, subject, body);
        }
    }
}
