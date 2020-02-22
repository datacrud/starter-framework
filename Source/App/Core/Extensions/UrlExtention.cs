using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project.Core.Extensions
{
    public static class UrlExtension
    {
        public static string GetHostUrl(this string url)
        {
            if (url.StartsWith("http://")) url = url.Replace("http://", "");
            if (url.StartsWith("www.")) url = url.Replace("www.", "");
            if (url.Contains("index.dev.html")) url = url.Replace("index.dev.html", "");

            return url;
        }


        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return $"{url.Scheme}://{url.Host}{port}{VirtualPathUtility.ToAbsolute(relativeUrl)}";
        }


        public static string ToAngularHashUri(this string uri)
        {
            //if (uri[uri.Length - 1] != '/') uri = uri + "/";
            uri = uri + "#!";

            return uri;
        }

        public static string PrefixAngularHashUri(this string uri)
        {
            if (uri[0] != '/') uri = "/" + uri;
            uri = "#!" + uri;

            return uri;
        }
    }
}
