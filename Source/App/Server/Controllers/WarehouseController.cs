using System;
using System.Web.Http;
using Project.Core.Enums.Framework;
using Project.Core.StaticResource;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.ViewModel;
using Security.Server.Providers;

namespace Project.Server.Controllers
{
    [Authorize]
    public class WarehouseController : HaveTenantIdCompanyIdControllerBase<Warehouse, WarehouseViewModel, WarehouseRequestModel>
    {
        private readonly IWarehouseService _service;
        private readonly IFeatureProvider _featureProvider;

        public WarehouseController(IWarehouseService service, IFeatureProvider featureProvider) : base(service)
        {
            _service = service;
            _featureProvider = featureProvider;
        }

        public override IHttpActionResult Post(Warehouse model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var featureWarehouseValue = _featureProvider.GetEditionFeatureValue(model.TenantId, StaticFeature.Warehouse.Name);
            bool isReachedMaximumWarehouseCount =
                _service.IsReachedMaximumWarehouseCount(model.TenantId, Convert.ToInt32(featureWarehouseValue));
            if (isReachedMaximumWarehouseCount) return BadRequest("You already have added " + featureWarehouseValue + " warehouse. You can not add more warehouse with your current subscription.");

            var isWarehouseNameExist = _service.IsWarehouseNameExist(model.Name, model.Id, ActionType.Put);
            if (isWarehouseNameExist) return BadRequest(model.Name + " warehouse name already exist.");

            var isWarehouseCodeExist = _service.IsWarehouseCodeExist(model.Code, model.Id, ActionType.Put);
            if (isWarehouseCodeExist) return BadRequest(model.Name + " warehouse code already exist.");

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            model.Active = true;

            _service.AddAsTenant(model);

            return Ok(model.Id);
        }

        public override IHttpActionResult Put(Warehouse model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isWarehouseNameExist = _service.IsWarehouseNameExist(model.Name, model.Id, ActionType.Put);
            if (isWarehouseNameExist) return BadRequest(model.Name + " warehouse name already exist.");

            var isWarehouseCodeExist = _service.IsWarehouseCodeExist(model.Code, model.Id, ActionType.Put);
            if (isWarehouseCodeExist) return BadRequest(model.Name + " warehouse code already exist.");

            _service.EditAsTenant(model);

            return Ok(model.Id);
        }
    }
}
