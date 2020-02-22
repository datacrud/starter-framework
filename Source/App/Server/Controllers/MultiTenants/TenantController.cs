using System;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Core.StaticResource;
using Project.Model;
using Project.Model.SeedData;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.Service.MultiTenants;
using Project.Service.Sms;
using Project.ViewModel;

namespace Project.Server.Controllers.MultiTenants
{
    [Authorize]
    [RoutePrefix("api/Tenant")]
    public class TenantController : ControllerBase<Tenant, TenantViewModel, TenantRequestModel>
    {
        private readonly ITenantService _service;
        private readonly ITenantManager _tenantManager;
        private readonly ISmsService _smsService;


        public TenantController(ITenantService service, 
            ITenantManager tenantManager,
            ISmsService smsService) : base(service)
        {
            _service = service;
            _tenantManager = tenantManager;
            _smsService = smsService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("CreateTenant")]
        public async Task<IHttpActionResult> CreateTenant(TenantViewModel model)
        {
            model.TenancyName = model.Name.ToTenancyName();
            if (!string.IsNullOrWhiteSpace(model.TenancyName))
            {
                ModelState.Remove("model.TenancyName");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isReservedName =_service.IsReservedName(model.TenancyName);
            if (isReservedName) return BadRequest($"Sorry, {model.TenancyName} is reserved. Please select another name");

            //if (model.SubscriptionEndTime == null) return BadRequest("Subscription end time can not be null. Please select a package.");

            var isTenantExist = _service.IsTenantExist(model.TenancyName);
            if (isTenantExist) return BadRequest("Company name '" + model.Name + "' already exist. Please try with different one.");

            //bool isEmailExist = _tenantManager.IsEmailExist(model.Email);
            //if (isEmailExist) return BadRequest(model.Email + " email address already exist.");

            if(model.PasswordHash != model.RetypePassword) return BadRequest("Password and confirm password could not match.");

            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            model.Active = true;

            if (model.IsLifeTimeSubscription) model.SubscriptionEndTime = null;

            string tenantId = model.Id;
            string companyId;
            string branchId;
            string subscriptionId;
            string adminRoleId;

            try
            {                
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.MaximumTimeout
                }))
                {
                    tenantId = _tenantManager.CreateTenant(model);

                    companyId = _tenantManager.CreateTenantCompany(tenantId, model.Name, model.Email, model.PhoneNumber);
                    //_tenantManager.CreateTenantCompanySettings(tenantId, company);

                    branchId = _tenantManager.CreateTenantHeadOfficeBranch(tenantId, companyId);
                    //_tenantManager.CreateTenantHeadOfficeWarehouse(tenantId, companyId);

                    subscriptionId = _tenantManager.CreateTenantSubscription(model, tenantId, companyId, branchId, model.IsLifeTimeSubscription);

                    _tenantManager.UpdateSubscriptionId(tenantId, subscriptionId);

                    //_tenantManager.CreateSupplier(tenantId, tenantCompanyId);


                    adminRoleId = _tenantManager.CreateTenantRole(tenantId, companyId);

                    scope.Complete();                    
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.ToString());

                await _tenantManager.RollbackAsync(tenantId);

                return BadRequest("Registration failed! please try again.");
            }


            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.MaxValue
                }))
                {

                    var userId = _tenantManager.CreateTenantAdminUser(model, tenantId, companyId, branchId, adminRoleId);
                    _tenantManager.CreateTenantAdminPermission(tenantId);

                    BusinessModelSeedDataManager.CreateFiscalYear(BusinessDbContext.Create(), tenantId);

                    scope.Complete();

                    var smsResponseModel = _smsService.SendOneToOneSingleSmsUsingApi(model.PhoneNumber, SmsHelper.TenantRegistrationMessage);

                    _tenantManager.ConfirmCompanyMobileNumber(smsResponseModel, companyId);
                    await _tenantManager.ConfirmAdminMobileNumberAsync(smsResponseModel, userId);
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.ToString());

                await _tenantManager.RollbackAsync(tenantId);

                return BadRequest("Registration failed! please try again.");
            }


            try
            {
                if (string.IsNullOrWhiteSpace(companyId) || string.IsNullOrWhiteSpace(tenantId)) throw new Exception("Company Id or Tenant Id can not be null");

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions()
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = TransactionManager.MaximumTimeout
                    }))
                {

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.ToString());

                await _tenantManager.RollbackAsync(tenantId);

                return BadRequest("Registration failed! please try again.");
            }



            return Ok(model);
        }


        public override IHttpActionResult Put(Tenant model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            model.Active = true;

            _service.EditAsHost(model);

            return Ok(model.Id);
        }



        public override IHttpActionResult Delete(DeleteActionType type, string id)
        {
            if (!User.IsInRole(StaticRoles.SystemAdmin.Name)) return BadRequest("Your are not authorized to delete tenant");

            IHttpActionResult actionResult;
            using (TransactionScope scope = new TransactionScope())
            {

                var run = Task.Run(async () => await _tenantManager.DeleteTenantDependencyAsync(id));
                run.Wait();

                actionResult = base.Delete(type, id);
                scope.Complete();
            }

            return actionResult;
        }
    }
}
