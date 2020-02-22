using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Server.Filters;
using Project.Service;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/Edition")]
    public class EditionController : ControllerBase<Edition, EditionViewModel, EditionRequestModel>
    {
        private readonly IEditionService _service;
        private readonly IFeatureService _featureService;

        public EditionController(IEditionService service, IFeatureService featureService) : base(service)
        {
            _service = service;
            _featureService = featureService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("LoadEdition")]
        public async Task<IHttpActionResult> LoadEdition(string id)
        {
            var edition = await _service.GetByIdAsync(id);
            var viewModel = new EditionViewModel(edition);
            return Ok(viewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("LoadEditions")]
        public async Task<IHttpActionResult> LoadEditions()
        {
            var viewModels = await _service.GetEditionsForSubscription();

            return Ok(viewModels);
        }

        public override IHttpActionResult Post(Edition model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isEditionExist = _service.IsEditionExist(model.Name, model.Id);
            if (isEditionExist) return BadRequest(model.Name + " edition already exist");

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            model.Active = true;

            Service.CreateAsHost(model);

            return Ok(model.Id);
        }

        public override IHttpActionResult Put(Edition model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isEditionExist = _service.IsEditionExist(model.Name, model.Id);
            if (isEditionExist) return BadRequest(model.Name + " edition already exist");

            Service.EditAsHost(model);
            if (model.EnableFeatureEdit)
                _featureService.Edit(model.Features.ToList(), model.Id);

            return Ok(model.Id);
        }
    }
}