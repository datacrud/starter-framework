using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Project.Core.Repositories;
using Project.Core.RequestModels;
using Project.Core.Session;
using Security.Server.Managers;
using Security.Server.Service;

namespace Security.Server.Controllers
{

    [Authorize]
    public abstract class SecurityControllerBase<TEntity> : ApiController where TEntity : class
    {

        protected readonly ISecurityServiceBase<TEntity> Service;
        protected readonly UserManager UserManager;
        protected readonly IAppSession AppSession;

        protected SecurityControllerBase(ISecurityServiceBase<TEntity> service)
        {
            Service = service;
            UserManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<UserManager>();
            AppSession = new AppSession();

            var tenantId = HttpContext.Current?.Request.Headers["TenantId"];
            UserManager?.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
        }


        public virtual IHttpActionResult Get()
        {
            var entities = Service.GetAll();

            return Ok(entities);
        }

        //public virtual IHttpActionResult Get([FromBody] PageFilterInput input)
        //{
        //    var entities = Service.GetAll(input);

        //    return Ok(entities);
        //}


        public virtual IHttpActionResult Get(string id)
        {
            var entity = Service.GetById(id);

            return Ok(entity);
        }


        public virtual IHttpActionResult Post(TEntity entity)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var add = Service.Create(entity);

            return Ok(add);
        }


        public virtual IHttpActionResult Put(TEntity model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var edit = Service.Update(model);

            return Ok(edit);
        }


        public virtual IHttpActionResult Delete(string request)
        {
            var delete = Service.Delete(request);

            return Ok(delete);
        }
    }
}
