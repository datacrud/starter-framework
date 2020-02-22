using System;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using Newtonsoft.Json;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Core.StaticResource;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.Service.Sms;
using Project.ViewModel;
using Serilog;

namespace Project.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/SubscriptionPayment")]
    public class SubscriptionPaymentController : HaveTenantIdCompanyIdControllerBase<SubscriptionPayment, SubscriptionPaymentViewModel, SubscriptionPaymentRequestModel>
    {
        private readonly ISubscriptionPaymentService _service;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ISmsService _smsService;

        public SubscriptionPaymentController(ISubscriptionPaymentService service, ISubscriptionService subscriptionService, ISmsService smsService) : base(service)
        {
            _service = service;
            _subscriptionService = subscriptionService;
            _smsService = smsService;
        }

        [HttpGet]
        [Route("GetTenantPayment")]
        public IHttpActionResult GetTenantPayment(PagingDataType status, string request)
        {
            var requestModel = JsonConvert.DeserializeObject<SubscriptionPaymentRequestModel>(request);

            var responseModel = _service.GetAllAsTenant(requestModel);

            return Ok(responseModel);
        }

        [HttpPost]
        [Route("Confirm")]
        public IHttpActionResult Confirm(PaymentConfirmationRequestModel requestModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using (var scope = new TransactionScope())
            {
                var company = _service.GetCompany(requestModel.SubscriptionId);
                string message = null;

                var verifyPaymentAndUpdateStatus = _service.ConfirmPaymentAndUpdateStatus(requestModel);

                if (verifyPaymentAndUpdateStatus == true)
                {
                    message = SmsHelper.PaymentConfirmedMessage;
                    _subscriptionService.UpdateSubscriptionStatus(requestModel.SubscriptionId, true);

                    if (company.IsEmailConfirmed)
                        _service.SendPaymentConfirmationEmail(requestModel.SubscriptionId, company.Email, company.Name);
                }

                if (verifyPaymentAndUpdateStatus == false)
                {
                    message = SmsHelper.PaymentFailedMessage;

                    if (company.IsEmailConfirmed)
                        _service.SendPaymentRejectionEmail(requestModel.SubscriptionId, company.Email, company.Name);
                }


                if (company.IsPhoneConfirmed)
                {
                    _smsService.SendOneToOneSingleSmsUsingApi(company.Phone, message);
                }

                if (verifyPaymentAndUpdateStatus == false)
                {
                    return BadRequest("Transaction number is not valid. Failed to verify payment.");
                }

                scope.Complete();
            }


            return Ok(true);
        }

        public override IHttpActionResult Post(SubscriptionPayment model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();

            model.Active = true;

            using (var scope = new TransactionScope())
            {
                try
                {

                    _service.AddAsTenant(model);
                    _subscriptionService.UpdatePaymentStatus(model.SubscriptionId,
                        SubscriptionPaymentStatus.AwaitingConfirmation);
                }
                catch (Exception e)
                {
                    Log.Information(e.Message);
                    _service.Delete(model.Id);

                    _subscriptionService.UpdatePaymentStatus(model.SubscriptionId,
                        SubscriptionPaymentStatus.Unpaid);
                }

                scope.Complete();
            }

            var company = _service.GetCompany(model.SubscriptionId);
            if (company != null)
            {
                _smsService.SendOneToOneSingleSmsUsingApi(company.Phone,
                    SmsHelper.PaymentReceivedMessage);
                if (company.IsEmailConfirmed)
                    _service.SendPaymentInvoiceEmail(model.Id, company.Email, company.Name);
            }

            return Ok(model.Id);
        }

        public override IHttpActionResult Get(PagingDataType status, string request)
        {
            var requestModel = JsonConvert.DeserializeObject<SubscriptionPaymentRequestModel>(request);

            var isSystemAdminUser = User.IsInRole(StaticRoles.SystemAdmin.Name);

            var responseModel = isSystemAdminUser
                ? _service.GetAll(requestModel)
                : _service.GetAllAsTenant(requestModel);

            return Ok(responseModel);
        }


        public override IHttpActionResult Get(SearchType type, PagingDataType status, string request)
        {
            var isSystemAdminUser = User.IsInRole(StaticRoles.SystemAdmin.Name);
            var entities = isSystemAdminUser
                ? Service.Search(status, JsonConvert.DeserializeObject<SubscriptionPaymentRequestModel>(request))
                : Service.SearchAsTenant(status, JsonConvert.DeserializeObject<SubscriptionPaymentRequestModel>(request));

            return Ok(entities);
        }


        public override IHttpActionResult Delete(DeleteActionType type, string id)
        {
            var payment = _service.GetByIdAsNoTracking(id);

            var httpActionResult = base.Delete(type, id);

            _subscriptionService.UpdatePaymentStatus(payment.SubscriptionId, SubscriptionPaymentStatus.Unpaid);

            return httpActionResult;
        }
    }
}