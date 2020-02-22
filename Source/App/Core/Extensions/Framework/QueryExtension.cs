using System.Linq;
using Project.Core.DomainBase;
using Project.Core.Repositories;
using Project.Core.Session;

namespace Project.Core.Extensions.Framework
{
    public static class QueryExtension
    {
        public static IQueryable<T> FilterTenant<T>(this IQueryable<T> source, string tenantId = null) where T: class, IHaveTenant<string>
        {
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                IAppSession session = new AppSession();
                tenantId = session.TenantId;
            }
            return source.Where(x => x.TenantId == tenantId);
        }

       

        public static IQueryable<T> FilterCompany<T>(this IQueryable<T> source) where T : class, IHaveCompany<string>
        {
            IAppSession session = new AppSession();
            return source.Where(x => x.CompanyId == session.CompanyId);
        }

        public static IQueryable<T> FilterBranch<T>(this IQueryable<T> source) where T : class, IHaveBranch<string>
        {
            IAppSession session = new AppSession();
            return source.Where(x => x.BranchId == session.BranchId);
        }
    }
}