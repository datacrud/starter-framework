namespace Project.ViewModel
{
    public class SmsResponseModel
    {

        public string Response { get; set; }
        public bool IsSuccess { get; }
        public string ResponseId { get; }
        public string ResponseCode { get; }
        public string RecipientNumbers { get; set; }        
        public string ErrorCode { get; }
        public string ErrorMessage { get; set; }


        public bool? IsDeliveryPending { get;}
        public bool? IsDeliveredSuccess { get;}


        public SmsResponseModel(string response)
        {
            Response = response;

            var replace = Response.Replace("||", "?");
            var split = replace.Split('?');
            if (split.Length == 3)
            {
                ResponseCode = split[0];
                RecipientNumbers = split[1];
                ResponseId = split[2].Replace("/", "");
            }            
            else if (split.Length == 1)
            {
                ResponseCode = split[0];
            }

            IsSuccess = ResponseCode == "1900";

            ErrorCode = IsSuccess == false ? ResponseCode : null;
            ErrorMessage = GetErrorMessage(ErrorCode);

            if (ResponseCode == "1" || ResponseCode == "2" || ResponseCode == "3")
            {
                IsDeliveryPending = ResponseCode == "1";
                IsDeliveredSuccess = ResponseCode == "2";
            }
        }

        private string GetErrorMessage(string errorCode)
        {
            switch (errorCode)
            {
                case "1901":
                    return "Parameter content missing";
                case "1902":
                    return "Invalid user/pass";
                case "1903":
                    return "Not enough balance";
                case "1905":
                    return "Invalid destination number";
                case "1906":
                    return "Operator Not found";
                case "1907":
                    return "Invalid mask Name";
                case "1908":
                    return "Sms body too long";
                case "1909":
                    return "Duplicate campaign Name";
                case "1910":
                    return "Invalid message";
                case "1911":
                    return "Too many Sms Request. Please try less than 500 in one request";
                case "1912":
                    return "Invalid sms content";
            }

            return null;
        }
    }
}