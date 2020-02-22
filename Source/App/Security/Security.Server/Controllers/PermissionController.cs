using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Server.Service;

namespace Security.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/Permission")]
    public class PermissionController : SecurityControllerBase<Permission>
    {
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service) : base(service)
        {
            _service = service;
        }


        [HttpPost]
        [Route("CheckPermission")]
        public IHttpActionResult CheckPermission(PermissionRequestModel model)
        {
            bool isPermitted = _service.CheckPermission(model);

            return Ok(isPermitted);
        }

        [HttpGet]
        [Route("GetListById")]
        public IHttpActionResult GetListById(string roleId)
        {
            //var permissions = _service.GetListById(request);

            var permissionTree = _service.GetPermissionTree(roleId);

            var serializeObject = JsonConvert.SerializeObject(permissionTree, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return Ok(serializeObject);
        }


        [HttpPost]
        [Route("AddList")]
        public IHttpActionResult Post(List<Permission> models)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            foreach (var model in models)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Ok(_service.AddList(models));
        }


    }
}