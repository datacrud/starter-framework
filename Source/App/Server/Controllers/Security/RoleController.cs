using System;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Server.Service;

namespace Project.Server.Controllers.Security
{
    [Authorize(Roles = "SystemAdmin, Admin")]
    [RoutePrefix("api/Role")]
    public class RoleController : SecurityBaseController<IdentityRole>
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service) :base(service)
        {
            _service = service;
        }

        public override IHttpActionResult Post(IdentityRole model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            model.Id = Guid.NewGuid().ToString();
            var add = _service.Add(model);

            return Ok(add);
        }


    }
}
