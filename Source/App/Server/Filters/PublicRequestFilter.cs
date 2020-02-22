using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace Project.Server.Filters
{
    [Obsolete]
    public class PublicRequestFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var isEnablePublicRequestFilter = ConfigurationManager.AppSettings["EnablePublicRequestFilter"];
            var host = ConfigurationManager.AppSettings["ValidHost"];

            if (actionContext.Request.Headers.Referrer != null)
            {
                var referrerHost = actionContext.Request.Headers.Referrer.Host;
                if (Convert.ToBoolean(isEnablePublicRequestFilter) && referrerHost.ToLower() != host.ToLower())
                {
                    var response =
                        actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "Invalid request source");
                    actionContext.Response = response;
                }
            }
            else
            {
                var requestUriHost = actionContext.Request.RequestUri.Host;

                if (Convert.ToBoolean(isEnablePublicRequestFilter) && requestUriHost.ToLower() != host.ToLower())
                {
                    var response =
                        actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request source");
                    actionContext.Response = response;
                }
            }

        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent != null)
            {
                var type = objectContent.ObjectType; //type of the returned object
                var value = objectContent.Value; //holding the returned value
            }


        }
    }
}