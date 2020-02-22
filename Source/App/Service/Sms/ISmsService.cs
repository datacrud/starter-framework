using Project.ViewModel;

namespace Project.Service.Sms
{
    public interface ISmsService
    {
        SmsResponseModel SendOneToOneSingleSms(string sendTo, string smsText);
        SmsResponseModel SendOneToOneSingleSmsUsingApi(string sendTo, string smsText);
        SmsResponseModel GetDeliveryStatusUsingApi(string responseId);
    }
}