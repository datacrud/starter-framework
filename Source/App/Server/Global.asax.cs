using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using Project.Server.Filters;

namespace Project.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);            
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }

        //public override void Init()
        //{
        //    PostAuthenticateRequest += Application_PostAuthenticateRequest;
        //    base.Init();
        //}

        //protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        //}

        //protected void Application_PostAuthorizeRequest()
        //{
        //    if (IsWebApiRequest())
        //    {
        //        HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        //    }
        //}

        //private bool IsWebApiRequest()
        //{
        //    return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null &&
        //           HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig
        //               .UrlPrefixRelative);
        //}
    }
}
