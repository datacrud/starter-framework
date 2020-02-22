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
    [Authorize]
    public class FiscalYearController : HaveTenantIdCompanyIdBranchIdControllerBase<FiscalYear, FiscalYearViewModel, FiscalYearRequestModel>
    {
        private readonly IFiscalYearService _service;
        private readonly ICompanyService _companyService;

        public FiscalYearController(IFiscalYearService service, ICompanyService companyService) : base(service)
        {
            _service = service;
            _companyService = companyService;
        }


        public override IHttpActionResult Post(FiscalYear model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!model.StartDate.HasValue) return BadRequest("Start date is required.");
            if (!model.EndDate.HasValue) return BadRequest("End date is required.");

            DateTime startDate, endDate;
            var tryParseS = DateTime.TryParse(model.StartDate.GetValueOrDefault().ToLongDateString(), out startDate);
            var tryParseE = DateTime.TryParse(model.EndDate.GetValueOrDefault().ToLongDateString(), out endDate);
            if (!tryParseS || !tryParseE) return BadRequest("You inputed value is not date.");

            if (model.EndDate.GetValueOrDefault().Date <= model.StartDate.GetValueOrDefault().Date)
                return BadRequest("End date must be grater than start date.");

            var company = _companyService.GetById(model.CompanyId);
            model.TenantId = company.TenantId;

            bool isExist = _service.IsExistFiscalYear(model.StartDate.GetValueOrDefault(),
                model.EndDate.GetValueOrDefault(), model.Id, model.TenantId);

            if (isExist)
            {
                FiscalYear fiscalYear = _service.GetFiscalYear(model.StartDate.GetValueOrDefault(),
                    model.EndDate.GetValueOrDefault(), model.TenantId);
                return BadRequest("Fiscal year already exist within date range " +
                                  fiscalYear.StartDate.GetValueOrDefault().ToShortDateString() + " to " +
                                  fiscalYear.EndDate.GetValueOrDefault().ToShortDateString());
            }
            

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();            
            model.Active = true;

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

        public override IHttpActionResult Put(FiscalYear model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!model.StartDate.HasValue) return BadRequest("Start date is required.");
            if (!model.EndDate.HasValue) return BadRequest("End date is required.");

            DateTime startDate, endDate;
            var tryParseS = DateTime.TryParse(model.StartDate.GetValueOrDefault().ToLongDateString(), out startDate);
            var tryParseE = DateTime.TryParse(model.EndDate.GetValueOrDefault().ToLongDateString(), out endDate);
            if (!tryParseS || !tryParseE) return BadRequest("You inputed value is not date.");

            if (model.EndDate.GetValueOrDefault().Date <= model.StartDate.GetValueOrDefault().Date)
                return BadRequest("End date must be grater than start date.");

            var company = _companyService.GetById(model.CompanyId);
            model.TenantId = company.TenantId;

            bool isExist = _service.IsExistFiscalYear(model.StartDate.GetValueOrDefault(),
                model.EndDate.GetValueOrDefault(), model.Id, model.TenantId);
            if (isExist)
            {
                FiscalYear fiscalYear = _service.GetFiscalYear(model.StartDate.GetValueOrDefault(),
                    model.EndDate.GetValueOrDefault(), model.TenantId);
                return BadRequest("Fiscal year already exist within date range " +
                                  fiscalYear.StartDate.GetValueOrDefault().ToShortDateString() + " to " +
                                  fiscalYear.EndDate.GetValueOrDefault().ToShortDateString());
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