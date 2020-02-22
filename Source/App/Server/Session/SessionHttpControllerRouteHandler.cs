using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;

namespace Project.Server.Session
{
    public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }
}