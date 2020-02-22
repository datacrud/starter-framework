using System.Net;
using Project.Core.Extensions;

namespace Project.Core.Handlers
{
    public class HttpStatusCodeOutput
    {

        public HttpStatusCodeOutput(HttpStatusCode httpStatusCode)
        {
            Code = httpStatusCode;
        }

        public HttpStatusCode Code { get; set; }
        public string Name => Code.GetDescription();
    }
}