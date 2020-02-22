namespace Project.BindingModel.Emailing
{
    public class SenderEmailModel
    {
        public bool IsModifiedEmail { get; set; }

        public string UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }

        public string BranchCode { get; set; }
        public string BranchName { get; set; }

        public string TenantId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmailAddress { get; set; }


        public string Sender { get; set; }
        public string Receiver { get; set; }
        public bool IsReceiverEmailVerified { get; set; }

    }
}
