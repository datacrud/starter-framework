using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Core.Session;
using Project.Core.StaticResource;
using Project.Model;
using Project.Model.EntityBase;
using Project.Model.EntityBases;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel.Bases;
using Security.Server.Managers;

namespace Project.Server.Controllers.Bases
{
    [Authorize]
    public abstract class ControllerBase : ApiController
    {
        protected readonly IAppSession AppSession;
        protected readonly UserManager UserManager;

        protected ControllerBase()
        {
            UserManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<UserManager>();
            AppSession = new AppSession();

            var tenantId = HttpContext.Current?.Request.Headers["TenantId"];
            UserManager?.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
        }
    }

    [Authorize]
    public abstract class ControllerBase<TEntity, TVm, TRm> : ControllerBase, IControllerBase<TEntity>
        where TEntity : BusinessEntityBase
        where TVm : ViewModelBase<TEntity>
        where TRm : RequestModelBase<TEntity>
    {
        protected IServiceBase<TEntity, TVm> Service;


        protected ControllerBase(IServiceBase<TEntity, TVm> service)
        {
            Service = service;
        }


        /// <summary>
        /// Get paging entries
        /// </summary>
        /// <param name="status"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IHttpActionResult Get(PagingDataType status, string request)
        {
            var requestModel = JsonConvert.DeserializeObject<TRm>(request);

            var responseModel = Service.GetAll(requestModel);

            return Ok(responseModel);
        }

        public virtual IHttpActionResult Get(SearchType type, PagingDataType status, string request)
        {
            var entities = Service.Search(status, JsonConvert.DeserializeObject<TRm>(request));
            return Ok(entities);
        }
        

        /// <summary>
        /// Get all entries
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IHttpActionResult Get(DataType type)
        {
            var list = Service.GetAll();
            return Ok(list);
        }


        /// <summary>
        /// Get single or default entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IHttpActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Id can not be null or empty or white space");
            var entity = Service.GetById(id);

            return Ok(entity);
        }

        /// <summary>
        /// Create an entry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IHttpActionResult Post(TEntity model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();

            model.Active = true;
            Service.CreateAsHost(model);

            return Ok(model.Id);
        }


        /// <summary>
        /// Update and entry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IHttpActionResult Put(TEntity model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Service.EditAsHost(model);

            return Ok(model.Id);
        }


        /// <summary>
        /// Delete an entry
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IHttpActionResult Delete(DeleteActionType type, string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Id can not be null or empty or white space");

            bool delete;
            switch (type)
            {
                case DeleteActionType.Delete:
                    delete = Service.Delete(id);
                    break;
                case DeleteActionType.Trash:
                    delete = Service.Trash(id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return Ok(delete);
        }

        /// <summary>
        /// Check is system admin user
        /// </summary>
        /// <returns></returns>
        protected bool IsSystemAdminUser()
        {
            return User.IsInRole(StaticRoles.SystemAdmin.Name);
        }

        protected HttpResponseMessage FileResponse(string filePath, string fileName)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(filePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return result;
        }


        /// <summary>
        /// Create entity audit log
        /// </summary>
        /// <param name="model"></param>
        /// <param name="invoiceNo"></param>
        /// <param name="entityType"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        protected AuditLog CreateAuditLog(TEntity model, string invoiceNo, EntityType entityType, ActionType actionType)
        {
            var transactionLog = new AuditLog()
            {
                Active = true,
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                EntityId = model.Id,
                EntityType = entityType,
                EntityInvoiceNo = invoiceNo,
                ActionType = actionType,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Modified = model.Modified,
                ModifiedBy = model.ModifiedBy,
                Data = SerializeToJson(model),
            };

            return transactionLog;
        }

        protected string SerializeToJson(TEntity entity)
        {
            var serializeObject = JsonConvert.SerializeObject(entity, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

            return serializeObject;
        }
    }
}