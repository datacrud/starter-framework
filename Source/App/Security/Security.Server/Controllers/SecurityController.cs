using System.Threading.Tasks;
using System.Web.Http;
using Project.Core.Extensions;
using Security.Models.RequestModels;
using Security.Server.Managers;
using Security.Server.Service;

namespace Security.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/Security")]
    public class SecurityController : ApiController
    {
        private readonly IUserService _userService;

        public SecurityController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(PasswordResetRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Your request is not valid.");

            if (model.NewPassword != model.ConfirmPassword) return BadRequest("Confirm password does not match");

            var user = await _userService.GetUserAsNoTrackingAsync(model.UserId);
            if (user == null) return BadRequest("Invalid user email verification request.");

            var result = await _userService.ResetPasswordAsync(user.Id, model.Code, model.NewPassword);

            return Ok(result.Succeeded);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ConfirmPassword")]
        public async Task<IHttpActionResult> ConfirmPassword(PasswordResetVerificationRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Your request is not valid.");

            model.Code = model.Code.ToIdentityGeneratedTokenFormat();

            bool isPasswordResetCodeExist = await _userService.IsPasswordResetCodeExist(model.Code, model.UserId);
            if (!isPasswordResetCodeExist) return BadRequest("Password reset code is not valid.");

            var user = await _userService.GetUserAsNoTrackingAsync(model.UserId);
            if (user == null) return BadRequest("Invalid user password reset request.");

            if (!string.Equals(user.PasswordChangeConfirmationCode, model.Code))
                return BadRequest("Password reset code is not valid.");


            return Ok(true);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IHttpActionResult> ForgetPassword(ForgetPasswordRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Your request is not valid.");
            var isEmailExist = _userService.IsEmailExist(model.Email);
            if (!isEmailExist) return BadRequest(model.Email + " email does not exist.");

            var user = await _userService.FindByEmailAsync(model.Email);
            if (user != null && !user.IsActive) return BadRequest("User is not active. Failed to send password reset link.");

            await _userService.SendPasswordResetLinkAsync(model.Email);

            return Ok(true);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("SendEmailConfirmationCode")]
        public async Task<IHttpActionResult> SendEmailConfirmationCode(ResendEmailVerificationRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Your request is not valid.");

            var isEmailExist = _userService.IsEmailExist(model.Email);
            if (!isEmailExist) return BadRequest(model.Email + " email does not exist.");

            var user = await _userService.FindByEmailAsync(model.Email);
            if (user != null && !user.IsActive) return BadRequest("User is not active. Failed to send activation email.");

            await _userService.ResendEmailVerificationCodeAsync(model.Email);

            return Ok(true);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> Post(EmailVerificationRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid email verification request.");

            model.Code = model.Code.ToIdentityGeneratedTokenFormat();

            bool isEmailVerificationCodeExist = await _userService.IsEmailVerificationCodeExist(model);
            if (!isEmailVerificationCodeExist) return BadRequest("Email confirmation code is not valid.");

            var user = await _userService.GetUserAsNoTrackingAsync(model.UserId);
            if (user == null) return BadRequest("Invalid user email verification request.");

            if (user.EmailConfirmed) return BadRequest("Email already verified.");

            if (!string.Equals(user.EmailConfirmationCode, model.Code))
                return BadRequest("Email confirmation code is not valid.");

            await _userService.ConfirmedEmailAsync(model.UserId, model.Code);

            return Ok(true);
        }

    }
}