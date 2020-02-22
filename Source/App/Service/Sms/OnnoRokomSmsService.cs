using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Service.com.onnorokomsms.api2;
using Project.ViewModel;

namespace Project.Service.Sms
{
    public class OnnoRokomSmsService : SmsServiceBase, ISmsService
    {


        private readonly string _smsApiKey;
        private readonly string _smsUserName;
        private readonly string _smsUserPassword;
        private readonly string _smsMaskName;
        private readonly string _smsCampaignName;
        private readonly string _smsTypeText;
        private readonly string _smsTypeUcs;
        

        public OnnoRokomSmsService()
        {
            
            _smsApiKey = ConfigurationManager.AppSettings["SMS:OnnoRokom:ApiKey"];
            _smsUserName = ConfigurationManager.AppSettings["SMS:OnnoRokom:Username"];
            _smsUserPassword = ConfigurationManager.AppSettings["SMS:OnnoRokom:Password"];
            _smsTypeText = ConfigurationManager.AppSettings["SMS:OnnoRokom:TextType"];
            _smsTypeUcs = ConfigurationManager.AppSettings["SMS:OnnoRokom:UcsType"];
            _smsMaskName = bool.Parse(ConfigurationManager.AppSettings["SMS:OnnoRokom:IsEnabledMask"])
                ? ConfigurationManager.AppSettings["SMS:OnnoRokom:MaskName"]
                : "";
            _smsCampaignName = ConfigurationManager.AppSettings["SMS:OnnoRokom:CampaignName"];
        }


        public SmsResponseModel SendOneToOneSingleSms(string sendTo, string smsText)
        {
            if (!IsEnabledSmsFeature) return null;

            SmsResponseModel smsResponse = null;
            try
            {
                var sms = new SendSms();                
                var response = sms.OneToOne(_smsUserName, _smsUserPassword, sendTo, smsText, _smsTypeText, _smsMaskName,
                    _smsCampaignName);

                smsResponse = new SmsResponseModel(response);

                SendLowBalanceAlertToManagement(sms);
            }
            catch (Exception ex)
            {
                // ignored
            }

            return smsResponse;
        }


        public SmsResponseModel SendOneToOneSingleSmsUsingApi(string sendTo, string smsText)
        {
            if (!IsEnabledSmsFeature) return null;

            SmsResponseModel smsResponse = null;
            try
            {
                var sms = new SendSms();
                var response = sms.NumberSms(_smsApiKey, smsText, sendTo, _smsTypeText, _smsMaskName, _smsCampaignName);

                smsResponse = new SmsResponseModel(response);

                SendLowBalanceAlertToManagementUsingApi(sms);
            }
            catch (Exception ex)
            {
                
                // ignored
            }

            return smsResponse;
        }

        public SmsResponseModel GetDeliveryStatusUsingApi(string responseId)
        {
            SmsResponseModel smsResponse = null;
            try
            {
                var sms = new SendSms();
                var response = sms.SMSDeliveryStatus(_smsApiKey, responseId);

                smsResponse = new SmsResponseModel(response);
            }
            catch (Exception ex)
            {

                // ignored
            }

            return smsResponse;
        }


        private void SendLowBalanceAlertToManagement(SendSms sms)
        {
            var balance = sms.GetBalance(_smsUserName, _smsUserPassword);
            if (double.Parse(balance) <= 2)
            {
                sms.OneToOne(_smsUserName, _smsUserPassword, SmsEmergencyContact, SmsEmergencyMessage,
                    _smsTypeText, _smsMaskName, _smsCampaignName);
            }
        }

        private void SendLowBalanceAlertToManagementUsingApi(SendSms sms)
        {
            var balance = sms.GetCurrentBalance(_smsApiKey);
            if (double.Parse(balance) <= 2)
            {
                sms.NumberSms(_smsApiKey, SmsEmergencyMessage, SmsEmergencyContact, _smsTypeText, _smsMaskName,
                    _smsCampaignName);
            }
        }
    }
}
