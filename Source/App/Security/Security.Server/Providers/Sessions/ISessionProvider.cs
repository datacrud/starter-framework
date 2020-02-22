using Project.ViewModel.Session;

namespace Security.Server.Providers.Sessions
{
    public interface ISessionProvider
    {
        SessionViewModel GetCurrentUserSession(string userId);
    }
}