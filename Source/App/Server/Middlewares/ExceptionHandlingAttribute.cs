using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Serilog;

namespace Project.Server.Middlewares
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //if (context.Exception is UserFriednlyException)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //    {
            //        Content = new StringContent(context.Exception.Message),
            //        ReasonPhrase = "Exception"
            //    });

            //}

            //Log Critical errors
            if (context.Exception != null) Log.Error(context.Exception.ToString());
            Log.CloseAndFlush();

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator."),
                ReasonPhrase = "Critical Exception"
            });
        }
    }
}