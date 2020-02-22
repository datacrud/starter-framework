using System;
using System.Web.Http;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.Service.AutoGenData;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    [Authorize]
    public class CustomerController : HaveTenantIdCompanyIdBranchIdControllerBase<Customer, CustomerViewModel, CustomerRequestModel>
    {
        private readonly ICustomerService _service;
        private readonly IAutoGenDataService _autoGenDataService;

        public CustomerController(ICustomerService service, 
            IAutoGenDataService autoGenDataService) : base(service)
        {
            _service = service;
            _autoGenDataService = autoGenDataService;
        }

        public override IHttpActionResult Post(Customer model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isCustomerPhoneExist = _service.IsCustomerPhoneExist(model.Phone, model.Id, ActionType.Post);
            if (isCustomerPhoneExist) return BadRequest("Customer phone no "+ model.Phone +  " already exist.");

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(model.BranchId)) model.BranchId = User.Identity.GetBranchId();
            model.Active = true;

            model.Code = _autoGenDataService.GetData(AutoGenType.CustomerCode);
            _service.CreateAsTenant(model);

            //_dailyCustomerAccountBalanceService.Create(entity.Id, entity.Created, entity.OpeningDue, User.Identity.GetUserId());

            return Ok(model.Id);
        }

        public override IHttpActionResult Put(Customer model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isCustomerPhoneExist = _service.IsCustomerPhoneExist(model.Phone, model.Id, ActionType.Post);
            if (isCustomerPhoneExist) return BadRequest("Customer phone no " + model.Phone + " already exist.");

            var edit = _service.EditAsTenant(model);

            return Ok(edit);
        }
    }
}
