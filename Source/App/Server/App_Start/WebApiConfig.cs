using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.WebHost;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Project.Core.Session;
using Project.Server.Middlewares;
using Project.Server.Providers.CorsPolicies;
using Project.Server.Session;
using Project.Service;

namespace Project.Server
{
    public static class WebApiConfig
    {
        public static string UrlPrefix => "api";
        public static string UrlPrefixRelative => "~/api";

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


            //Cors
            config.SetCorsPolicyProviderFactory(new CorsPolicyFactory());
            //config.SetCorsPolicyProviderFactory(new DynamicPolicyProviderFactory());
            config.EnableCors();
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*", "*"));

            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());


            ////Enable session state
            //var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance",
            //    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //if (httpControllerRouteHandler != null)
            //{
            //    httpControllerRouteHandler.SetValue(null,
            //        new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            //}


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
        }
    }
}
