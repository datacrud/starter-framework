using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Project.Server.Controllers.Bases
{
    public class ReportControllerBase : ApiController
    {
        protected HttpResponseMessage FileResponse(string filePath)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(filePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(filePath)
                
            };
            return result;
        }

        protected HttpResponseMessage FileResponse(string filePath, string fileName)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(filePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName

            };
            return result;
        }

        protected string GetPdfReportHeader(string name, DateTime from, DateTime to)
        {
            return name + " : From " + from.Day + " " +
                   from.ToString("MMM") + " " +
                   from.Year + " To " + to.Day + " " +
                   to.ToString("MMM") + " " +
                   to.Year;
        }
    }
}