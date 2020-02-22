using System;
using System.Security.Claims;
using System.Security.Principal;


namespace Project.Core.Extensions
{
    public static class IdentityExtension
    {

        public static string GetCurrentUserId(this IIdentity identity)
        {            
            var claim = ((ClaimsIdentity)identity)?.FindFirst("UserId");
            return claim?.Value;
        }
        public static string GetCurrentEmployeeId(this IIdentity identity)
        {
            try
            {
                var claim = ((ClaimsIdentity)identity)?.FindFirst("EmployeeId");
                return claim?.Value;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetCurrentUserName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity)?.FindFirst("UserName");
            return claim?.Value;
        }

        public static string GetCurrentUserEmail(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity)?.FindFirst("UserEmail");
            return claim?.Value;
        }

        public static string GetBranchId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity)?.FindFirst("BranchId");
            return claim?.Value;
        }


        public static string GetCompanyId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity)?.FindFirst("CompanyId");
            return claim?.Value;
        }

        public static string GetTenantId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity)?.FindFirst("TenantId");
            return claim?.Value;
        }

        public static string GetTenantName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity)?.FindFirst("TenantName");
            return claim?.Value;
        }
    }
}