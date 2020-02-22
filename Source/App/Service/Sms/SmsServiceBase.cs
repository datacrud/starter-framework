using System.Configuration;

namespace Project.Service.Sms
{
    public abstract class SmsServiceBase
    {
        protected readonly bool IsEnabledSmsFeature;
        protected readonly string SmsEmergencyContact;
        protected readonly string SmsEmergencyMessage;

        protected SmsServiceBase()
        {
            IsEnabledSmsFeature = bool.Parse(ConfigurationManager.AppSettings["SMS:IsEnabled"]);
            SmsEmergencyContact = ConfigurationManager.AppSettings["SMS:EmergencyContact"];
            SmsEmergencyMessage = ConfigurationManager.AppSettings["SMS:EmergencyMessage"];
        }
    }
}