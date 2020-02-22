using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Project.Core.RequestModels;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Server.Managers;
using Security.Server.Service;

namespace Security.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/Profile")]
    public class ProfileController : ApiController
    {
        private readonly IProfileService _service;

        public ProfileController(IProfileService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("ChangeProfilePicture")]
        public async Task<IHttpActionResult> ChangeProfilePicture()
        {
            return Ok();
        }

        [HttpGet]
        [Route("UserProfile")]
        public IHttpActionResult GetUserProfile()
        {
            var model = _service.GetUserProfile();
            return Ok(model);
        }

        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<IHttpActionResult> UpdateProfile(UserProfileUpdateRequestModel model)
        {
            if (model.IsChangeEmail && string.IsNullOrWhiteSpace(model.AwaitingConfirmEmail) == false)
            {
                var isEmailExist = await _service.IsAlreadyExistAsync(model.AwaitingConfirmEmail);
                if (isEmailExist)
                    return BadRequest(model.AwaitingConfirmEmail +
                                      " already exist. Please use another email address.");
            }

            return Ok(await _service.UpdateProfile(model));
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(PasswordChangeRequestModel model)
        {
            return Ok(_service.ChangePassword(model));
        }

        [HttpPost]
        [Route("SendEmailConfirmationCode")]
        public async Task<IHttpActionResult> SendEmailConfirmationCode(EmailConfirmationRequestModel model)
        {
            await _service.SendEmailConfirmationCode(model);

            return Ok(true);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(EmailConfirmationRequestModel model)
        {
            var result = await _service.ConfirmEmail(model);

            if (result.IsSuccess == false)
            {
                await _service.ResetProfile(model.Id);
            }

            return Ok(result);
        }


        [HttpPost]
        [Route("ResetProfile")]
        public async Task<IHttpActionResult> ResetProfile()
        {
            var identityResult = await _service.ResetProfile(User.Identity.GetUserId());
            return Ok(identityResult);
        }
    }
}
