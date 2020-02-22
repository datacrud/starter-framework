using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
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
    [RoutePrefix("api/ManageSubscription")]
    public class ManageSubscriptionController : HaveTenantIdCompanyIdControllerBase<Subscription, SubscriptionViewModel, SubscriptionRequestModel>
    {
        private readonly ISubscriptionService _service;
        private readonly ITenantService _tenantService;
        private readonly ISubscriptionPaymentService _subscriptionPaymentService;

        public ManageSubscriptionController(ISubscriptionService service, 
            ITenantService tenantService, 
            ISubscriptionPaymentService subscriptionPaymentService) : base(service)
        {
            _service = service;
            _tenantService = tenantService;
            _subscriptionPaymentService = subscriptionPaymentService;
        }


        [HttpGet]
        [Route("GetTenantSubscriptions")]
        public IHttpActionResult GetTenantSubscriptions()
        {
            List<SubscriptionViewModel> subscriptions = _service.GetTenantSubscriptions(User.Identity.GetTenantId());

            var viewModels = subscriptions.OrderByDescending(x => x.ExpireOn)
                .ToList();

            return Ok(viewModels);
        }

        [HttpGet]
        [Route("GetTenantCurrentSubscription")]
        public IHttpActionResult GetTenantCurrentSubscription()
        {
            var viewModel = _tenantService.GetTenantCurrentSubscription(User.Identity.GetTenantId());

            return Ok(viewModel);
        }


        public override IHttpActionResult Post(Subscription model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(model.TenantId)) model.TenantId = User.Identity.GetTenantId();
            if (string.IsNullOrWhiteSpace(model.CompanyId)) model.CompanyId = User.Identity.GetCompanyId();
            model.Active = true;

            using (var scope = new TransactionScope())
            {
                _service.AddAsTenant(model);
                UpdateDependency(model);

                scope.Complete();
            }


            return Ok(model.Id);
        }


        public override IHttpActionResult Put(Subscription model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using (var scope = new TransactionScope())
            {
                _service.EditAsTenant(model);
                UpdateDependency(model);

                scope.Complete();
            }


            return Ok(model.Id);
        }

        //public override IHttpActionResult Delete(DeleteActionType type, string id)
        //{
        //    IHttpActionResult httpActionResult;

        //    var subscription = _service.GetEntityByIdAsNoTracking(id);
        //    var tenantId = subscription.TenantId;

        //    using (var scope = new TransactionScope())
        //    {

        //        _subscriptionPaymentService.DeleteSubscriptionPayments(id);

        //        httpActionResult = base.Delete(type, id);

        //        _service.RevertPreviousSubscriptionRenewedOn(tenantId);
        //        _tenantService.RevertTenantSubscriptionInfo(tenantId);

        //        scope.Complete();
        //    }

        //    return httpActionResult;
        //}

        private void UpdateDependency(Subscription entity)
        {
            _tenantService.UpdateTenantSubscriptionInfo(entity.TenantId, entity.EditionId, entity.Id);
        }

    }
}
