using System.Configuration;

namespace Project.Core.StaticResource
{
    public class SmsHelper
    {
        public static string GetSupportNo()
        {
            return ConfigurationManager.AppSettings["App:SupportContact"];
        }

        public static readonly string SubscriptionExpireMessage =
            $"Your subscription will expire tomorrow. Thanks, {AppSettings.PoweredBy.ToLower()}";

        public static readonly string TenantRegistrationMessage =
            $"Congratulation, you have registered on {AppSettings.ApplicationName.ToLower()} successfully. For any query call " + GetSupportNo();

        public static readonly string PaymentReceivedMessage =
            $"We have received your payment. You will get a confirmation sms soon. Thanks,  {AppSettings.PoweredBy.ToLower()}";

        public static readonly string PaymentConfirmedMessage =
            $"Congratulation, your payment has been verified successfully. Thanks,  {AppSettings.PoweredBy.ToLower()}";

        public static readonly string PaymentFailedMessage =
            "Sorry, your payment failed to verify. Recheck your transaction id. For further query call " +
            GetSupportNo();

    }
}