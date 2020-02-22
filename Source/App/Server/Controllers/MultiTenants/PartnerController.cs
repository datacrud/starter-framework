using System;
using System.Web.Http;
using Project.Core.Extensions;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    public class PartnerController : HaveTenantIdCompanyIdBranchIdControllerBase<Partner, PartnerViewModel, PartnerRequestModel>
    {
        private readonly IPartnerService _service;
        private readonly ICompanyService _companyService;

        public PartnerController(IPartnerService service, ICompanyService companyService) : base(service)
        {
            _service = service;
            _companyService = companyService;
        }

        public override IHttpActionResult Post(Partner model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();            
            model.Active = true;

            var company = _companyService.GetById(model.CompanyId);
            model.TenantId = company.TenantId;

            if (IsSystemAdminUser() && model.IsHostAction)
            {
                Service.CreateAsHost(model);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.BranchId)) model.BranchId = User.Identity.GetBranchId();
                Service.CreateAsTenant(model);
            }

            return Ok(model.Id);
        }

        public override IHttpActionResult Put(Partner model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var company = _companyService.GetById(model.CompanyId);
            model.TenantId = company.TenantId;

            if (IsSystemAdminUser() && model.IsHostAction)
            {
                Service.EditAsHost(model);
            }
            else
            {
                Service.EditAsTenant(model);
            }

            return Ok(model.Id);
        }
    }
}