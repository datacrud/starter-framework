using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http.Cors;
using Project.Core;
using Project.Core.Extensions;
using Project.Model;
using Project.Server.Providers.CorsPolicies;

namespace Project.Server.Middlewares.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CustomCorsPolicyAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public CustomCorsPolicyAttribute()
        {
            // Create a CORS policy.
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };
            
        }

        
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isAllowed = false;

            var corsRequestContext = request.GetCorsRequestContext();
            string origin = corsRequestContext.Origin;

            var corsPolicies = origin.Contains(AppConst.Localhost)
                ? ConfigurationManager.AppSettings["App:CorsPolicies:Dev"]
                : ConfigurationManager.AppSettings["App:CorsPolicies:Prod"];

            if (corsPolicies.Split(",").Any(x => x.Trim() == origin))
            {
                isAllowed = true;
            }
            else
            {
                var strings = origin.GetHostUrl().Split(".");
                if (strings.Length > 2)
                {
                    using (var db = new BusinessDbContext())
                    {
                        var tenancyName = strings[0];
                        isAllowed = db.Tenants.AsNoTracking().Any(x => x.TenancyName == tenancyName && x.IsActive);
                    }
                }
            }


            if (isAllowed)
            {
                // Create a CORS policy.
                _policy = new CorsPolicy
                {
                    AllowAnyMethod = true,
                    AllowAnyHeader = true,
                };

                _policy.Origins.Add(corsRequestContext.Origin);
            }

            return Task.FromResult(_policy);
        }
    }
}