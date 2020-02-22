using System.Security.Principal;

namespace Project.Core.CurrentUser
{
    public interface ICurrentUser
    {
        IPrincipal User { get; set; }
    }
}