using System.Collections.Generic;
using System.Text.RegularExpressions;
using Project.Core.Extensions;

namespace Project.Core.Helpers
{
    public class TenantHelper
    {
        public static string BuildTenancyName(string name)
        {
            return Regex.Replace(name, @"\s+", "").ToLower();
        }

        public static string GetTenantNameFromReferer(string url)
        {
            if (url.Contains("https://"))
            {
                url = url.Replace("https://", "");
            }
            if (url.Contains("http://"))
            {
                url = url.Replace("http://", "");
            }
            if (url.Contains("www."))
            {
                url = url.Replace("www.", "");
            }
            if (url.Contains("index.dev.min.html"))
            {
                url = url.Replace("index.dev.min.html", "");
            }
            if (url.Contains("index.dev.html"))
            {
                url = url.Replace("index.dev.html", "");
            }
            if (url.Contains("index.html"))
            {
                url = url.Replace("index.html", "");
            }

            if (string.IsNullOrWhiteSpace(url)) return null;

            var split = url.Split('.');
            return split.Length > 2 ? split[0] : null;
        }

        public static List<string> GetReservedNames()
        {
            return  new List<string>
            {
                "datacrud",
                "tenant",
                "company",
                "app",
                "host",
                "production",
                "staging",
                "development",
                "deployment",
                "deploy",
                "demo",
                "test",
                "learn",
                "console",
                "portal"
            };
        }

        public static string GetTenantBaseUrl(string tenancyName)
        {
            tenancyName = tenancyName.ToTenancyName();
            return AppConst.AppBaseUrl.Replace(AppConst.TenancyName, tenancyName);
        }
    }
}