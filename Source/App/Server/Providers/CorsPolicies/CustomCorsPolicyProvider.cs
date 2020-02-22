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

namespace Project.Server.Providers.CorsPolicies
{
    public class CustomCorsPolicyProvider : ICorsPolicyProvider 
    {
        private CorsPolicy _policy;

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isAllowed = false;

            var corsRequestContext = request.GetCorsRequestContext();
            string origin = corsRequestContext.Origin;

            var corsPolicies = origin.Contains(AppConst.Localhost)
                ? ConfigurationManager.AppSettings["App:CorsPolicies:Dev"]
                : ConfigurationManager.AppSettings["App:CorsPolicies:Prod"];

            if (corsPolicies.Split(",").Any(x=> x.Trim() == origin))
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
                    SupportsCredentials = true,
                };

                _policy.Origins.Add(corsRequestContext.Origin);
            }

            return Task.FromResult(_policy);
        }
    }








    #region Dynamci Cors Policy Factory

    public class DynamicPolicyProviderFactory : ICorsPolicyProviderFactory
    {
        public ICorsPolicyProvider GetCorsPolicyProvider(
            HttpRequestMessage request)
        {
            var route = request.GetRouteData();
            var controller = (string)route.Values["controller"];
            var corsRequestContext = request.GetCorsRequestContext();
            var originRequested = corsRequestContext.Origin;
            var policy = GetPolicyForControllerAndOrigin(
                controller, originRequested);
            return new CustomPolicyProvider(policy);
        }
        private CorsPolicy GetPolicyForControllerAndOrigin(
            string controller, string originRequested)
        {
            // Do database lookup to determine if the controller is allowed for
            // the origin and create CorsPolicy if it is (otherwise return null)
            var policy = new CorsPolicy();
            policy.Origins.Add(originRequested);
            policy.Methods.Add("GET");
            return policy;
        }
    }
    public class CustomPolicyProvider : ICorsPolicyProvider
    {
        CorsPolicy policy;
        public CustomPolicyProvider(CorsPolicy policy)
        {
            this.policy = policy;
        }
        public Task<CorsPolicy> GetCorsPolicyAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.policy);
        }
    }


    #endregion
}