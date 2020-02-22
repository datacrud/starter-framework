using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Dapper;
using Project.Core.Extensions;

namespace Project.Core.Session
{
    public class AppSession : IAppSession
    {
        public string UserId => GetAppSession().UserId;
        public string EmployeeId => GetAppSession().EmployeeId;
        public string UserName => GetAppSession().UserName;
        public string UserEmail => GetAppSession().UserEmail;
        public string TenantId => GetAppSession().TenantId;
        public string TenantName => GetAppSession().TenantName;
        public string CompanyId => GetAppSession().CompanyId;
        public string BranchId => GetAppSession().BranchId;




        public AppSession()
        {

        }

        private AppSessionModel GetAppSession()
        {
            //var authentication = HttpContext.Current.GetOwinContext().Authentication;
            //var claims = authentication.User.Claims.ToList();

            var identity = HttpContext.Current?.User.Identity;

            var tenantId = identity?.GetTenantId();
            var tenantName = identity?.GetTenantName();

            var companyId = identity?.GetCompanyId();
            var branchId = identity?.GetBranchId();

            var model = new AppSessionModel()
            {
                UserId = identity.GetCurrentUserId(),
                EmployeeId = identity.GetCurrentEmployeeId(),
                UserName = identity.GetCurrentUserName(),
                UserEmail = identity.GetCurrentUserEmail(),
                BranchId = !string.IsNullOrWhiteSpace(branchId) ? branchId : null,
                CompanyId = !string.IsNullOrWhiteSpace(companyId) ? companyId : null,
                TenantName = !string.IsNullOrWhiteSpace(tenantName) ? tenantName : null,
                TenantId = !string.IsNullOrWhiteSpace(tenantId) ? tenantId : null,
            };

            return model;
        }


      
    }
}
