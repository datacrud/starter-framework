using System;
using System.Linq;
using System.Web.Http;
using Project.Server.Models;
using Project.Server.Service;

namespace Project.Server.Controllers.Security
{
    [Authorize(Roles = "SystemAdmin")]
    [RoutePrefix("api/Resource")]
    public class ResourceController : SecurityBaseController<SecurityModels.AspNetResource>
    {
        private readonly IResourceService _service;

        public ResourceController(IResourceService service) :base(service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("GetPrivateResources")]
        public IHttpActionResult GetPrivateResources()
        {
            var resources = _service.GetAll().Where(x => x.IsPublic == false).OrderBy(x => x.Name);

            return Ok(resources);
        }


        public override IHttpActionResult Post(SecurityModels.AspNetResource model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            model.Id = Guid.NewGuid().ToString();
            var add = _service.Add(model);

            return Ok(add);
        }

    }
}
