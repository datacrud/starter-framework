namespace Project.ViewModel.Session
{
    public class SessionViewModel
    {
        public UserSessionViewModel User { get; set; } = new UserSessionViewModel();
        public SubscriptionSessionViewModel Subscription { get; set; } = new SubscriptionSessionViewModel();
        public CompanyViewModel Company { get; set; }
        public ApplicationSessionViewModel Application { get; set; } = new ApplicationSessionViewModel();
        
    }
}