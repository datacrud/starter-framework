namespace Security.Models.RequestModels
{
    public class UserProfileUpdateRequestModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsChangeEmail { get; set; }
        public string AwaitingConfirmEmail { get; set; }
        public bool IsAwaitingConfirmEmail { get; set; }
        public string ConfirmationCode { get; set; }
    }
}