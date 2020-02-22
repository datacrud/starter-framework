using System.Web.Http;
using Project.Server.Filters;

namespace Project.Server.Controllers.Securities
{

    [TenantCheckerFilter]
    public class TenantCheckerController : ApiController
    {
        public IHttpActionResult Post(object o)
        {
            if (User.Identity.IsAuthenticated)
            {
                
            }

            return Ok(true);
        }


    }
}
