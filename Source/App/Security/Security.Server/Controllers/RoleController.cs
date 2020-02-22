using System;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Core.RequestModels;
using Project.Core.StaticResource;
using Security.Models.Models;
using Security.Server.Managers;
using Security.Server.Service;

namespace Security.Server.Controllers
{
    [RoutePrefix("api/Role")]
    public class RoleController : SecurityControllerBase<Role>
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService service) : base(service)
        {
            _roleService = service;
        }

        [HttpGet, Route("GetTenantRoles")]
        public IHttpActionResult GetTenantRoles(string tenantId)
        {
            var roles = _roleService.GetTenantRoles(tenantId);

            return Ok(roles);
        }

    }
}