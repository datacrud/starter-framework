namespace Project.Core.RequestModels
{
    public class EmailConfirmationRequestModel
    {
        public EmailConfirmationRequestModel()
        {
            IsResend = false;
            IsQueryAsTracking = true;
        }

        public string Id { get; set; }
        public string ConfirmationCode { get; set; }
        public bool IsResend { get; set; }

        public bool IsQueryAsTracking { get; set; }
    }
}
