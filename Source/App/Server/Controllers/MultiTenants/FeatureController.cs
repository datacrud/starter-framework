using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Project.Core.Enums;
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
    public class FeatureController : ControllerBase<Feature, FeatureViewModel, FeatureRequestModel>
    {
        private readonly IFeatureService _service;

        public FeatureController(IFeatureService service) : base(service)
        {
            _service = service;
        }

        public override IHttpActionResult Post(Feature model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isFeatureExist = _service.IsFeatureExist(model.Name, model.Id, ActionType.Post);
            if (isFeatureExist) return BadRequest(model.Name + " feature already exist.");

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            model.Active = true;            
            
            _service.CreateAsHost(model);

            return Ok(model.Id);
        }


        public override IHttpActionResult Put(Feature model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isFeatureExist = _service.IsFeatureExist(model.Name, model.Id, ActionType.Put);
            if (isFeatureExist) return BadRequest(model.Name + " feature already exist.");

            return Ok(_service.EditAsHost(model));
        }
    }
}
