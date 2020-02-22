using System;
using System.Linq;
using System.Web.Http;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Core.StaticResource;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Server.Providers;
using Project.Service;
using Project.ViewModel;
using Security.Server.Providers;

namespace Project.Server.Controllers
{
    [Authorize]
    public class BranchController : HaveTenantIdCompanyIdControllerBase<Branch, BranchViewModel, BranchRequestModel>
    {
        private readonly IBranchService _service;
        private readonly ICompanyService _companyService;
        private readonly IFeatureProvider _featureProvider;
        private readonly IWarehouseService _warehouseService;

        public BranchController(IBranchService service, 
            ICompanyService companyService,
            IFeatureProvider featureProvider,
            IWarehouseService warehouseService) : base(service)
        {
            _service = service;
            _companyService = companyService;
            _featureProvider = featureProvider;
            _warehouseService = warehouseService;
        }


        public override IHttpActionResult Post(Branch model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var company = _companyService.GetById(model.CompanyId);
            model.TenantId = company.TenantId;

            var featureBranchCount = _featureProvider.GetEditionFeatureValue(model.TenantId, StaticFeature.Showroom.Name);
            bool isReachedMaximumBranch =
                _service.IsReachedMaximumBranchCount(model.TenantId, Convert.ToInt32(featureBranchCount));
            if(isReachedMaximumBranch) return BadRequest("You already have added " + featureBranchCount + " branch. You can not add more branch with your current subscription.");

            if (model.Type == BranchType.HeadOffice)
            {
                bool isHeadOfficeBranchExist = _service.IsHeadOfficeExist(model.Type, model.Id, model.TenantId);
                if (isHeadOfficeBranchExist) return BadRequest("Head office  branch type already exist. You can not add duplicate head office.");
            }

            var isBranchExist = _service.IsBranchExist(model.Name, model.Id, model.TenantId);
            if (isBranchExist) return BadRequest(model.Name + " branch name already exist");


            if (string.IsNullOrWhiteSpace(model.LinkedWarehouseId))
            {
                var warehouseId = Guid.NewGuid().ToString();
                var warehouse = new Warehouse
                {
                    Id = warehouseId,
                    Code = model.Code,
                    Name = model.Name,
                    Address = model.Address,
                    Created = model.Created,
                    CreatedBy = model.CreatedBy,
                    Modified = model.Modified,
                    ModifiedBy = model.ModifiedBy,
                    Active = true,
                    Type = model.Type == BranchType.HeadOffice
                        ? WarehouseType.HeadOffice
                        : WarehouseType.Showroom,
                };


                if (IsSystemAdminUser() && model.IsHostAction)
                {
                    _warehouseService.CreateAsHost(warehouse);
                }
                else
                {
                    _warehouseService.AddAsTenant(warehouse);
                }

                model.LinkedWarehouseId = warehouseId;
            }

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            model.Active = true;

            if (IsSystemAdminUser() && model.IsHostAction)
            {                
                Service.CreateAsHost(model);
            }
            else            
            {
                Service.AddAsTenant(model);
            }

            return Ok(model.Id);
        }

        public override IHttpActionResult Put(Branch model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var company = _companyService.GetById(model.CompanyId);
            model.TenantId = company.TenantId;

            if (model.Type == BranchType.HeadOffice)
            {
                bool isHeadOfficeBranchExist = _service.IsHeadOfficeExist(model.Type, model.Id, model.TenantId);
                if (isHeadOfficeBranchExist) return BadRequest("Head office  branch type already exist. You can not add duplicate head office.");
            }

            var isBranchExist = _service.IsBranchExist(model.Name, model.Id, model.TenantId);
            if (isBranchExist) return BadRequest(model.Name + " branch name already exist");

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