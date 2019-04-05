using System;
using System.Collections.Generic;
using System.Web.Http;
using Project.Server.Models;
using Project.Server.Service;

namespace Project.Server.Controllers.Security
{
    [Authorize]
    [RoutePrefix("api/Permission")]
    public class PermissionController : SecurityBaseController<SecurityModels.AspNetPermission>
    {
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service) : base(service)
        {
            _service = service;
        }
        

        [HttpPost]
        [Route("CheckPermission")]
        public IHttpActionResult CheckPermission(RequestModels.PermissionRequestModel model)
        {
            bool isPermitted = _service.CheckPermission(model);

            return Ok(isPermitted);
        }

        [HttpGet]
        [Route("GetListById")]
        public IHttpActionResult GetListById(string request)
        {
            var permissions = _service.GetListById(request);

            return Ok(permissions);
        }        


        [HttpPost]
        [Route("AddList")]
        public IHttpActionResult Post(List<SecurityModels.AspNetPermission> models)
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
