using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.ViewModel;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Project.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/Subscription")]
    public class SubscriptionController : HaveTenantIdCompanyIdControllerBase<Subscription, SubscriptionViewModel, SubscriptionRequestModel>
    {
        private readonly ISubscriptionService _service;
        private readonly ITenantService _tenantService;
        private readonly ISubscriptionPaymentService _subscriptionPaymentService;

        public SubscriptionController(ISubscriptionService service, ITenantService tenantService, ISubscriptionPaymentService subscriptionPaymentService) : base(service)
        {
            _service = service;
            _tenantService = tenantService;
            _subscriptionPaymentService = subscriptionPaymentService;
        }

        public override IHttpActionResult Post(Subscription model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(model.TenantId)) model.TenantId = User.Identity.GetTenantId();
            model.Active = true;

            try
            {
                using (var scope = new TransactionScope())
                {
                    model.ExpireOn = model.ExpireOn.GetValueOrDefault().Date.AddDays(1).AddSeconds(-1);
                    _service.AddAsTenant(model);
                    UpdateDependance(model);

                    scope.Complete();
                }

            }
            catch (Exception exception)
            {
                Log.Logger.Write(LogEventLevel.Error, exception, String.Empty);
            }

            return Ok(model.Id);
        }        


        public override IHttpActionResult Put(Subscription model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            using (var scope = new TransactionScope())
            {
                _service.EditAsTenant(model);
                UpdateDependance(model);

                scope.Complete();
            }
           

            return Ok(model.Id);
        }


        private void UpdateDependance(Subscription entity)
        {
            _tenantService.UpdateTenantSubscriptionInfo(entity.TenantId, entity.EditionId, entity.Id);
        }
    }
}
