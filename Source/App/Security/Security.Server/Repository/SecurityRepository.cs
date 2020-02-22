using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Project.Core.DomainBase;
using Project.Core.Extensions.Framework;
using Project.Core.Repositories;
using Project.Core.Session;
using Security.Models.Models;
using Security.Server.Managers;

namespace Security.Server.Repository
{
    public class SecurityRepository<TEntity, TKey> : GenericRepository<SecurityDbContext, TEntity, TKey>, ISecurityRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, IHaveTenant<string>, IHaveCompany<string>
    {

        protected SecurityDbContext DbContext;
        protected readonly UserManager UserManager;
        protected readonly IAppSession AppSession;

        public SecurityRepository() : base(new SecurityDbContext())
        {
            UserManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<UserManager>();
            AppSession = new AppSession();

            var tenantId = HttpContext.Current?.Request.Headers["TenantId"];
            UserManager?.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
        }


        public override IQueryable<TEntity> GetAll()
        {
            return base.GetAll().FilterTenant();
        }


        public IQueryable<TEntity> GetAllIgnoreFilter()
        {
            return base.GetAll();
        }
    }


}