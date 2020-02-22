using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    [Authorize]
    public class SupplierController : HaveTenantIdCompanyIdBranchIdControllerBase<Supplier, SupplierViewModel, SupplierRequestModel>
    {
        private readonly ISupplierService _service;
        public SupplierController(ISupplierService service) : base(service)
        {
            _service = service;
        }

        public override IHttpActionResult Post(Supplier model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            model.Active = true;

            _service.CreateAsTenant(model);

            return Ok(model.Id);
        }
    }
}
