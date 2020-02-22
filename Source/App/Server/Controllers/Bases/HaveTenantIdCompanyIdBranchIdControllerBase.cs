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
using Project.Core.Extensions;
using Project.Core.Session;
using Project.Core.StaticResource;
using Project.Model;
using Project.Model.EntityBases;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel.Bases;
using Security.Server.Managers;

namespace Project.Server.Controllers.Bases
{
    class Instruction
    {
        //Host Implementation Instruction
        //1. Override Post and Put on controller and User *AsHost instead of *AsTenant.
        //2. Override Repository Get*AsTenant and remove the tenant filters from methods.
    }


    [Authorize]
    public abstract class HaveTenantIdCompanyIdBranchIdControllerBase<TEntity, TVm, TRm> : ApiController, IHaveTenantIdCompanyIdBranchIdControllerBase<TEntity>
        where TEntity : HaveTenantIdCompanyIdBranchIdEntityBase
        where TVm : HaveTenantIdCompanyIdBranchIdViewModelBase<TEntity>
        where TRm : HaveTenantIdCompanyIdBranchIdRequestModelBase<TEntity>
    {
        protected IHaveTenantIdCompanyIdBranchIdServiceBase<TEntity, TVm> Service;

        protected readonly UserManager UserManager;

        protected readonly IAppSession AppSession;


        protected HaveTenantIdCompanyIdBranchIdControllerBase(IHaveTenantIdCompanyIdBranchIdServiceBase<TEntity, TVm> service)
        {
            Service = service;

            UserManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<UserManager>();
            AppSession = new AppSession();

            var tenantId = HttpContext.Current?.Request.Headers["TenantId"];
            UserManager?.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
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
            if (string.IsNullOrWhiteSpace(model.BranchId)) model.BranchId = User.Identity.GetBranchId();
            model.Active = true;

            Service.CreateAsTenant(model);

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
                BranchId = model.BranchId,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Modified = model.Modified,
                ModifiedBy = model.ModifiedBy,
                Data = SerializeToJson(model),
                TenantId = model.TenantId,
                CompanyId =  model.CompanyId,
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