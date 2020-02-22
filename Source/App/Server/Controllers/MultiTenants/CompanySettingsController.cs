
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.Service.MultiTenants;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    [RoutePrefix("api/CompanySettings")]
    public class CompanySettingsController : HaveTenantIdCompanyIdControllerBase<CompanySetting, CompanySettingsViewModel, CompanySettingsRequestModel>
    {
        private readonly ICompanySettingsService _service;
        private readonly ICompanyService _companyService;

        public CompanySettingsController(ICompanySettingsService service,
            ICompanyService companyService) : base(service)
        {
            _service = service;
            _companyService = companyService;
        }


        [Route("GetByCompanyId")]
        public IHttpActionResult GetByCompanyId(string companyId)
        {
            CompanySettingsViewModel companySettings = _service.GetByCompanyId(companyId);
            return Ok(companySettings);
        }

        public override IHttpActionResult Put(CompanySetting model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (model.IsUseDefaultSettings)
            {
                var company = _companyService.GetById(model.CompanyId);
                model.EmailSenderDisplayName = company.Name;
                model.NotificationSenderEmail = company.Email;
            }


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
