using System;
using System.Web.Http;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    [Authorize]
    public class EmployeeController : HaveTenantIdCompanyIdBranchIdControllerBase<Employee, EmployeeViewModel, EmployeeRequestModel>
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service) : base(service)
        {
            _service = service;
        }

        public override IHttpActionResult Post(Employee model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isEmployeeCodeExist = _service.IsEmployeeCodeExist(model.Code, model.Id, ActionType.Post);
            if (isEmployeeCodeExist) return BadRequest(model.Name + " employee code already exist.");

            var isEmployeeNameExist = _service.IsEmployeeNameExist(model.Name, model.Id, ActionType.Post);
            if (isEmployeeNameExist) return BadRequest(model.Name + " employee name already exist.");

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            model.Active = true;

            if (model.IsHostAction)
            {
                _service.CreateAsHost(model);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.BranchId)) model.BranchId = User.Identity.GetBranchId();
                _service.CreateAsTenant(model);
            }

            return Ok(model.Id);
        }

        public override IHttpActionResult Put(Employee model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isEmployeeCodeExist = _service.IsEmployeeCodeExist(model.Code, model.Id, ActionType.Post);
            if (isEmployeeCodeExist) return BadRequest(model.Name + " employee code already exist.");

            var isEmployeeNameExist = _service.IsEmployeeNameExist(model.Name, model.Id, ActionType.Post);
            if (isEmployeeNameExist) return BadRequest(model.Name + " employee name already exist.");

            _service.EditAsTenant(model);

            return Ok(model.Id);
        }
    }
}
