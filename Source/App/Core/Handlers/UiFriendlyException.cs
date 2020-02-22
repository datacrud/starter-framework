using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace Project.Core.Handlers
{
    public class UiFriendlyException: Exception, IHttpActionResult
    {
        public HttpStatusCodeOutput Status { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public UiFriendlyException(HttpStatusCode status, string title, string details): base(details)
        {
            Status = new HttpStatusCodeOutput(status);

            Title = title;

            Details = details;
        }


        public UiFriendlyException()
        {
            Status = new HttpStatusCodeOutput(HttpStatusCode.InternalServerError);
            Title = base.Message;
            Details = base.StackTrace;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(Status.Code)
            {
                Content = new ObjectContent<UiFriendlyException>(this, new JsonMediaTypeFormatter()),
            };
            return Task.FromResult(response);
        }
    }
}
