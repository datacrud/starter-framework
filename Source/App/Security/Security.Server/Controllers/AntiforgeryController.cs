using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using Newtonsoft.Json;

namespace Security.Server.Controllers
{
    [RoutePrefix("api/Antiforgery")]
    public class AntiforgeryController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage GetAntiForgeryToken()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            HttpCookie cookie = HttpContext.Current.Request.Cookies["xsrf-token"];

            string cookieToken;
            string formToken;
            AntiForgery.GetTokens(cookie == null ? "" : cookie.Value, out cookieToken, out formToken);

            AntiForgeryTokenModel content = new AntiForgeryTokenModel
            {
                AntiForgeryToken = formToken
            };

            response.Content = new StringContent(
                JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(cookieToken))
            {
                response.Headers.AddCookies(new[]
                {
                    new CookieHeaderValue("xsrf-token", cookieToken)
                    {
                        Expires = DateTimeOffset.Now.AddMinutes(10),
                        Path = "/"
                    }
                });
            }

            return response;
        }
    }

    public class AntiForgeryTokenModel
    {
        public string AntiForgeryToken { get; set; }
    }
}