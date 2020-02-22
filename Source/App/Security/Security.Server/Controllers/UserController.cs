using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Project.Core.Extensions;
using Project.Core.RequestModels;
using Project.Core.StaticResource;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Server.Managers;
using Security.Server.Providers;
using Project.Core.Repositories;
using Project.Core.Session;
using Security.Server.Service;

namespace Security.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : SecurityControllerBase<User>
    {
        private readonly IUserService _service;
        private readonly ITenantProvider _tenantProvider;
        private readonly IFeatureProvider _featureProvider;
        private readonly ICompanyProvider _companyProvider;

        public UserController(IUserService service,
            ITenantProvider tenantProvider,
            IFeatureProvider featureProvider,
            ICompanyProvider companyProvider) : base(service)
        {
            _service = service;
            _tenantProvider = tenantProvider;
            _featureProvider = featureProvider;
            _companyProvider = companyProvider;
        }




        [HttpGet]
        [Route("GetFilterUsers")]
        public async Task<IHttpActionResult> GetFilterUsers(string request)
        {

            var requestModel = JsonConvert.DeserializeObject<UserFilterRequestModel>(request);
            List<User> users = null;
            if (requestModel != null)
            {
                if (!string.IsNullOrWhiteSpace(requestModel.TenantId))
                {
                    users = await _service.GetFilterUser(requestModel.TenantId);
                }
                else
                {
                    users = await _service.GetUsersAsync();
                }
            }

            return Ok(users);
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IHttpActionResult> GetUsers()
        {
            var users = await _service.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetTenantUsers")]
        public async Task<IHttpActionResult> GetTenantUsers()
        {
            var users = await _service.GetUsersByTenantIdAsync(User.Identity.GetTenantId());
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var user = await _service.GetUserAsync(id);
            if (string.IsNullOrWhiteSpace(user.CompanyId))
                user.CompanyId = _companyProvider.GetAll().FirstOrDefault(x => x.TenantId == user.TenantId)?.Id;
            return Ok(user);
        }


        [HttpPost]
        [Route("CreateUser")]
        public async Task<IHttpActionResult> CreateUser(UserCreateRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Id)) model.Id = Guid.NewGuid().ToString();
            model.UserName = model.Email;

            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.Remove("model.UserName");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_service.IsEmailExist(model.Email))
            {
                return BadRequest($"{model.Email} email address already exist.");
            }


            if (string.IsNullOrWhiteSpace(model.TenantId))
            {
                model.TenantId = User.Identity.GetTenantId();
                model.TenantName = User.Identity.GetTenantName();
                model.CompanyId = User.Identity.GetCompanyId();
            }
            else
            {
                var tenant = await _tenantProvider.GetTenantAsync(model.TenantId);
                var company = await _tenantProvider.GetTenantCompanyAsync(model.TenantId);
                model.TenantName = tenant.TenancyName;
                model.CompanyId = company?.Id;
            }

            model.CreatedBy = User.Identity.GetUserId();
            model.IsActive = !model.SendActivationEmailToUser;
            model.EmailConfirmed = !model.SendActivationEmailToUser;

            var featureUsers = _featureProvider.GetEditionFeatureValue(model.TenantId, StaticFeature.Users.Name);
            bool isReachedMaximumUsersCount =
                _service.IsReachedMaximumUsersCount(model.TenantId, Convert.ToInt32(featureUsers));
            if (isReachedMaximumUsersCount)
                return BadRequest("You already have added " + featureUsers + " users. You can not add more user with your current subscription.");

            var identityResult = await _service.CreateUserAsync(model);

            if (model.SendActivationEmailToUser)
            {
                var user = await _service.GetUserAsync(model.Id);

                user.EmailConfirmationCode = await UserManager.GenerateEmailConfirmationTokenAsync(model.Id);
                user.EmailConfirmed = false;
                user.EmailConfirmationCodeExpireTime = DateTime.Now.AddMinutes(30);
                user.PhoneConfirmationCode = await UserManager.GenerateChangePhoneNumberTokenAsync(model.Id, model.PhoneNumber);
                user.PhoneNumberConfirmed = false;
                user.PhoneConfirmationCodeExpireTime = DateTime.Now.AddMinutes(30);

                await _service.UpdateUserAsync(user);

                await _service.SendEmailConfirmationLinkAsync(model.Id, model.FullName(), model.Email, user.EmailConfirmationCode);
            }

            return Ok(identityResult);
        }


        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IHttpActionResult> UpdateUser(UserCreateRequestModel model)
        {
            
            model.ModifiedBy = User.Identity.GetUserId();
            model.Modified = DateTime.Now;

            var isInSystemAdminRole = User.IsInRole(StaticRoles.SystemAdmin.Name);
            if (isInSystemAdminRole) return Ok(await _service.UpdateUserAsync(model));

            if (string.IsNullOrWhiteSpace(model.TenantId))
            {
                model.TenantId = User.Identity.GetTenantId();
                model.TenantName = User.Identity.GetTenantName();
                model.CompanyId = User.Identity.GetCompanyId();
            }
            else
            {
                var tenant = await _tenantProvider.GetTenantAsync(model.TenantId);
                model.TenantName = tenant.TenancyName;
                model.CompanyId = _companyProvider.GetAll().FirstOrDefault(x => x.TenantId == tenant.Id)?.Id;
            }


            return Ok(await _service.UpdateUserAsync(model));
        }


        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            if (HttpContext.Current.User.Identity.GetUserId() == id)
                return BadRequest("You can not delete your own access.");

            var isLastUser = _service.IsLastUser(id);
            if (isLastUser) return BadRequest("You can not delete the last user of " + User.Identity.GetTenantName());

            return Ok(await _service.DeleteUserAsync(id));
        }


    }
}
