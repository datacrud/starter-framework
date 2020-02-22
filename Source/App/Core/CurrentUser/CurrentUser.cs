using System.Security.Principal;
using System.Web;

namespace Project.Core.CurrentUser
{
    public class CurrentUser : ICurrentUser
    {
        public IPrincipal User
        {
            get => HttpContext.Current.User;
            set => HttpContext.Current.User = value;
        }
    }
}