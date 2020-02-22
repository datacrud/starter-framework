namespace Security.Models.RequestModels
{
    public class PasswordChangeRequestModel
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string RetypePassword { get; set; }
    }
}