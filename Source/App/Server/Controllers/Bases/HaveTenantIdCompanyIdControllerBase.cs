using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Core.StaticResource;
using Project.Model;
using Project.Model.EntityBase;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel.Bases;

namespace Project.Server.Controllers.Bases
{

    [Authorize]
    public abstract class HaveTenantIdCompanyIdControllerBase<TEntity, TVm, TRm> : ApiController, IHaveTenantIdCompanyIdControllerBase<TEntity>
        where TEntity : HaveTenantIdCompanyIdEntityBase
        where TVm : HaveTenantIdCompanyIdViewModelBase<TEntity>
        where TRm : HaveTenantIdCompanyIdRequestModelBase<TEntity>
    {
        protected IHaveTenantIdCompanyIdServiceBase<TEntity, TVm> Service;

        protected HaveTenantIdCompanyIdControllerBase(IHaveTenantIdCompanyIdServiceBase<TEntity, TVm> service)
        {
            Service = service;
        }


        //==================
        //Paging Block Start
        //==================
        public virtual IHttpActionResult Get(PagingDataType status, string request)
        {
            var requestModel = JsonConvert.DeserializeObject<TRm>(request);

            var responseModel = Service.GetAllAsTenant(requestModel);

            return Ok(responseModel);
        }

        public virtual IHttpActionResult Get(SearchType type, PagingDataType status, string request)
        {
            var entities = Service.SearchAsTenant(status, JsonConvert.DeserializeObject<TRm>(request));
            return Ok(entities);
        }
        //================
        //Paging Block End
        //================


        public virtual IHttpActionResult Get(DataType type)
        {
            var list = Service.GetAllAsTenant();
            return Ok(list);
        }


        public virtual IHttpActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Id can not be null or empty or white space");
            var entity = Service.GetById(id);

            return Ok(entity);
        }


        public virtual IHttpActionResult Post(TEntity model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();

            model.Active = true;
            Service.AddAsTenant(model);

            return Ok(model.Id);
        }


        public virtual IHttpActionResult Put(TEntity model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Service.EditAsTenant(model);

            return Ok(model.Id);
        }


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



        protected AuditLog CreateAuditLog(TEntity model, string invoiceNo, EntityType entityType, ActionType actionType)
        {
            var auditLog = new AuditLog()
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
                TenantId = model.TenantId,
                CompanyId = model.CompanyId,
            };

            return auditLog;
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