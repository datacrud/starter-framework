using System;
using System.Configuration;
using System.Web;
using Project.Core.Extensions;

namespace Project.Core.Helpers
{
    public class EmailHelper
    {
        public class EmailAddress
        {
            public class Sales
            {
                public const string Email = "sales@datacrud.com";
                public const string Name = "DataCrud Sales Team";
            }
            public class Support
            {
                public const string Email = "support@datacrud.com";
                public const string Name = "DataCrud Support";
            }
            public class Info
            {
                public const string Email = "info@datacrud.com";
                public const string Name = "DataCrud Info";
            }

            public class Noreply
            {
                public const string Email = "noreply@datacrud.com";
                public const string Name = "DataCrud No Reply";
            }

            public class Admin
            {
                public const string Email = "admin@datacrud.com";
                public const string Name = "DataCrud Admin";
            }
        }

        public class Signature
        {
            public static readonly string Sales = $"<br/><br/>{EmailAddress.Sales.Name}<br/>Contact: {EmailAddress.Sales.Email}<br/>2017 - {DateTime.Today.Year} &copy; DataCrud (a product of www.datacrud.com), All rights reserved.";


        }


        public static string GenerateVerificationCode()
        {
            Random random = new Random();
            int verificationCode = random.Next(10000000, 99999999);

            return verificationCode.ToString();
        }

        public static string GetBaseUri()
        {
            string uri;

            var useWebConfigEmailBaseUri = bool.Parse(ConfigurationManager.AppSettings["Emailing:IsUseWebConfigEmailBaseUri"]);            
            if (useWebConfigEmailBaseUri)
            {
                uri = ConfigurationManager.AppSettings["Emailing:BaseUri"];
            }
            else
            {
                try
                {
                    uri = "http://datacrud.com";
                    if (HttpContext.Current != null)
                    {
                        uri = HttpContext.Current.Request.UrlReferrer?.AbsoluteUri;

                        if (string.IsNullOrWhiteSpace(uri))
                        {
                            uri = HttpContext.Current.Request.Url.AbsoluteUri;
                        }
                    }
                }
                catch (Exception)
                {
                    uri = "http://datacrud.com";
                }              

            }

            if (string.IsNullOrWhiteSpace(uri)) uri = "http://datacrud.com";

            var usePlainUrl = bool.Parse(ConfigurationManager.AppSettings["WebClient:IsUsePlainUrl"]);
            if (uri.Contains("index.dev.html") && usePlainUrl == false)
            {
                var length = uri.Length;
                if (uri[length - 1] == '/') uri = uri.Remove(length, 1);
                uri = uri.ToAngularHashUri();
            }
            else if (uri.Contains("index.html") && usePlainUrl == false)
            {
                var length = uri.Length;
                if (uri[length - 1] == '/') uri = uri.Remove(length, 1);
                uri = uri.ToAngularHashUri();
            }
            else
            {
                uri = uri.ToAngularHashUri();
            }   

            return uri;
        }


        public static string ResetPasswordUri()
        {
            var path = StaticResource.StaticResource.Public.ResetPassword.Path;
            if (path[0] != '/') path = "/" + path;
            return GetBaseUri() + path;
        }

        public static string EmailVerificationUri()
        {
            var path = StaticResource.StaticResource.Public.EmailVerification.Path;
            if (path[0] != '/') path = "/" + path;
            return GetBaseUri() + path;
        }

        public static string ManageSubscriptionUri()
        {
            var path = StaticResource.StaticResource.Private.MultiTenant.ManageSubscription.Path;
            if (path[0] != '/') path = "/" + path;
            return GetBaseUri() + path;
        }
    }
}