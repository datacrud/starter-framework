using System.Net.Http;
using System.Web.Http.Filters;
using Project.Core.Handlers;

namespace Project.Server.Filters
{
    public class UiFriendlyExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //Check the Exception Type

            if (actionExecutedContext.Exception is UiFriendlyException)
            {
                //The Response Message Set by the Action During Ececution
                var res = actionExecutedContext.Exception.Message;

                //Define the Response Message
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    Content = new StringContent(res),
                    ReasonPhrase = res
                };


                //Create the Error Response

                actionExecutedContext.Response = response;
            }
        }
    }
}