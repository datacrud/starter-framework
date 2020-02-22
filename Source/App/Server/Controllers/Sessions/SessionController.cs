using System.Web.Http;
using Microsoft.AspNet.Identity;
using Security.Server.Providers.Sessions;

namespace Project.Server.Controllers.Sessions
{
    [RoutePrefix("api/Session")]
    public class SessionController : ApiController
    {
        private readonly ISessionProvider _sessionProvider;

        public SessionController(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        [HttpGet, Route("GetCurrentUserSession")]
        public IHttpActionResult GetCurrentUserSession()
        {

            var currentUserSession = _sessionProvider.GetCurrentUserSession(User.Identity.GetUserId());

            return Ok(currentUserSession);
        }
    }
}
