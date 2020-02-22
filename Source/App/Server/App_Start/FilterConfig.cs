using System.Web.Mvc;
using Project.Core.Handlers;
using Project.Server.Filters;

namespace Project.Server
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new UiFriendlyExceptionFilterAttribute());
            //filters.Add(new TenantCheckerFilter());
        }
    }
}
