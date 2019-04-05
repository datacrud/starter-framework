using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Http;
using Project.Model;
using Project.Server.Controllers.Security;
using Project.Service;

namespace Project.Server.Controllers
{
    public interface IBaseController<T> where T: Entity
    {
        IHttpActionResult Get();
        IHttpActionResult Get(string id);
        //IHttpActionResult Get(string keyword, bool isAssending, string orderBy);
        IHttpActionResult Post(T entity);
        IHttpActionResult Post(List<T> entities);
        IHttpActionResult Put(T entity);
        IHttpActionResult Delete(string id);
        IHttpActionResult Trash(string id);
    }

    public abstract class BaseController<TEntity> : ApiController, IBaseController<TEntity> where TEntity : Entity 
    {

        protected IBaseService<TEntity> Service;

        protected BaseController(IBaseService<TEntity> service)
        {
            Service = service;
        }


        public virtual IHttpActionResult Get()
        {
            var entities = Service.GetAll();

            return Ok(entities);
        }


        public virtual IHttpActionResult Get(string id)
        {
            var entity = Service.GetById(id);

            return Ok(entity);
        }


        public virtual IHttpActionResult Post(TEntity entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            entity.Id = Guid.NewGuid().ToString();
            var add = Service.Add(entity);

            return Ok(add);
        }



        [HttpPost]
        [ActionName("SaveEntries")]
        public IHttpActionResult Post(List<TEntity> entities)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (TEntity entity in entities)
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            bool addRanges = Service.Add(entities);

            return Ok(addRanges);
        }


        public virtual IHttpActionResult Put(TEntity model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var edit = Service.Edit(model);

            return Ok(edit);
        }


        public virtual IHttpActionResult Delete(string id)
        {
            var delete = Service.Delete(id);
            return Ok(delete);
        }


        [HttpDelete]
        [ActionName("Trash")]
        public IHttpActionResult Trash(string id)
        {
            bool trash = Service.Trash(id);
            return Ok(trash);
        }
    }
}
