using System.Web.Http;
using Project.Server.Service;
using Project.Service;

namespace Project.Server.Controllers.Security
{
    public interface ISecurityBaseController<in T> where T: class
    {
        IHttpActionResult Get();
        IHttpActionResult Get(string request);
        IHttpActionResult Post(T entity);
        IHttpActionResult Put(T entity);
        IHttpActionResult Delete(string request);
    }

    public abstract class SecurityBaseController<TEntity> : ApiController, ISecurityBaseController<TEntity> where TEntity : class 
    {

        protected ISecurityBaseService<TEntity> Service;

        protected SecurityBaseController(ISecurityBaseService<TEntity> service)
        {
            Service = service;
        }


        public virtual IHttpActionResult Get()
        {
            var entities = Service.GetAll();

            return Ok(entities);
        }


        public virtual IHttpActionResult Get(string request)
        {
            var entity = Service.GetById(request);

            return Ok(entity);
        }


        public virtual IHttpActionResult Post(TEntity entity)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var add = Service.Add(entity);

            return Ok(add);
        }


        public virtual IHttpActionResult Put(TEntity model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var edit = Service.Edit(model);

            return Ok(edit);
        }


        public virtual IHttpActionResult Delete(string request)
        {
            var delete = Service.Delete(request);

            return Ok(delete);
        }
    }
}
