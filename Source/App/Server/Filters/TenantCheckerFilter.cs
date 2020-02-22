using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Project.Core.Extensions;
using Project.Model;
using Project.Service;
using Serilog;

namespace Project.Server.Filters
{
    public class TenantCheckerFilter  : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(HttpActionContext actionContext)
        {


            if (actionContext.Request.Headers.Referrer != null)
            {
                var host = actionContext.Request.Headers.Referrer.Host;
                host = host.GetHostUrl();
                var split = host.Split('.');
                if (split.Length > 2)
                {
                    string tenancyName = split[0].ToLower();
                    Tenant tenant;

                    using (var db = new BusinessDbContext())
                    {
                        tenant = db.Tenants.AsNoTracking().FirstOrDefault(x => x.TenancyName.ToLower() == tenancyName);
                        db.Dispose();
                    }

                    if (tenant == null)
                    {                        
                        var response =
                            actionContext.Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                "Sorry, '"+ split[0].ToUpper() + "' is not registered to us.");
                        actionContext.Response = response;

                        Log.Error(actionContext.Response.ToString());
                    }
                    if (tenant != null && !tenant.Active || tenant != null && tenant.IsDeleted)
                    {
                        var response =
                            actionContext.Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                "Sorry, '" + split[0].ToUpper() + "' is not registered to us.");
                        actionContext.Response = response;

                        Log.Error(actionContext.Response.ToString());
                    }
                }                
            }



            base.OnActionExecuting(actionContext);
        }
    }
}