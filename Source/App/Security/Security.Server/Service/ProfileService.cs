using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.Core.Email;
using Project.Core.RequestModels;
using Project.Core.StaticResource;
using Project.ViewModel;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Models.ViewModels;
using Security.Server.Managers;
using Security.Server.Repository;

namespace Security.Server.Service
{
    public class ProfileService : IProfileService
    {
        private readonly ISecurityRepository<User, string> _repository;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;
        private readonly UserManager _userManager;

        public ProfileService(ISecurityRepository<User, string> repository,
            IEmailSender emailSender,
            IEmailService emailService)
        {
            _repository = repository;
            _emailSender = emailSender;
            _emailService = emailService;
            _userManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<UserManager>();

            var tenantId = HttpContext.Current?.Request.Headers["TenantId"];
            _userManager?.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
        }



        public ProfileViewModel GetUserProfile()
        {
            var user = _userManager.FindById(HttpContext.Current.User.Identity.GetUserId());

            var viewModel = new ProfileViewModel(user);

            var roles = _userManager.GetRoles(user.Id);
            viewModel.RoleNames = roles.ToArray();
            viewModel.RoleName = string.Join(",", roles.Select(x => x));


            return viewModel;
        }


        public async Task<IdentityResult> UpdateProfile(UserProfileUpdateRequestModel model)
        {
            var user = _userManager.FindById(HttpContext.Current.User.Identity.GetUserId());

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.IsChangeEmail = model.IsChangeEmail;
            user.AwaitingConfirmEmail = model.AwaitingConfirmEmail;
            if (!user.IsChangeEmail) user.AwaitingConfirmEmail = "";
            if (user.AwaitingConfirmEmail.IsNullOrWhiteSpace()) user.IsChangeEmail = false;

            //if (user.IsChangeEmail && !string.IsNullOrWhiteSpace(user.AwaitingConfirmEmail))
            //{
            //    var isEmailExist = await _repository.GetAll().AsNoTracking().AnyAsync(x => x.Email == user.AwaitingConfirmEmail);

            //    if (isEmailExist == false)
            //    {

            //        user.EmailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        user.EmailConfirmed = true;
            //        user.EmailConfirmationCodeExpireTime = DateTime.Now.AddMinutes(30);
            //        await _emailService.SendConfirmationEmailToUser(user.AwaitingConfirmEmail, user.FullName(),
            //            user.EmailConfirmationCode, user.EmailConfirmationCodeExpireTime.GetValueOrDefault());
            //    }
            //}

            return await _userManager.UpdateAsync(user);
        }

        public bool ChangePassword(PasswordChangeRequestModel model)
        {
            if (model.NewPassword != model.RetypePassword) return false;

            var user = _userManager.FindById(HttpContext.Current.User.Identity.GetUserId());

            var verifyHashedPassword = new PasswordHasher().VerifyHashedPassword(user.PasswordHash, model.CurrentPassword);

            if (verifyHashedPassword == PasswordVerificationResult.Success)
            {
                user.PasswordHash = new PasswordHasher().HashPassword(model.NewPassword);

                _userManager.Update(user);
            }

            return true;
        }

        public async Task<EmailResponseModel> ConfirmEmail(EmailConfirmationRequestModel model)
        {
            EmailResponseModel response = new EmailResponseModel();

            if (!string.IsNullOrWhiteSpace(model.ConfirmationCode) && !string.IsNullOrWhiteSpace(model.Id))
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if (user.EmailConfirmationCodeExpireTime.HasValue &&
                    user.EmailConfirmationCodeExpireTime.GetValueOrDefault() < DateTime.Now)
                {
                    response.IsSuccess = false;
                    response.Message = "Your confirmation code has been expired.";
                }


                if (!string.IsNullOrWhiteSpace(user.EmailConfirmationCode) && string.Equals(model.ConfirmationCode?.ToLower(), user.EmailConfirmationCode?.ToLower()))
                {
                    user.Email = user.AwaitingConfirmEmail;

                    if (!user.UserName.IsReservedUsername())
                    {
                        user.UserName = user.Email;
                    }

                    user.IsChangeEmail = false;
                    user.AwaitingConfirmEmail = null;
                    user.EmailConfirmationCode = null;
                    user.EmailConfirmationCodeExpireTime = null;

                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Your confirmation code is not valid";
                }
            }

            return response;
        }

        public async Task SendEmailConfirmationCode(EmailConfirmationRequestModel model)
        {
            var user = _userManager.FindById(HttpContext.Current.User.Identity.GetUserId());

            if (user.IsChangeEmail && !string.IsNullOrWhiteSpace(user.AwaitingConfirmEmail))
            {
                if (string.IsNullOrWhiteSpace(user.EmailConfirmationCode) || model.IsResend)
                {
                    user.EmailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    user.EmailConfirmed = true;
                    user.EmailConfirmationCodeExpireTime = DateTime.Now.AddMinutes(30);
                    await _emailService.SendConfirmationEmailToUser(user.AwaitingConfirmEmail, user.FullName(),
                        user.EmailConfirmationCode, user.EmailConfirmationCodeExpireTime.GetValueOrDefault());

                    await _userManager.UpdateAsync(user);
                }
            }
        }

        public async Task<bool> IsAlreadyExistAsync(string email)
        {
            return await _repository.GetAll().AsNoTracking().AnyAsync(x => x.Email == email);
        }

        public async Task<IdentityResult> ResetProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsChangeEmail = false;

            user.EmailConfirmationCode = null;
            user.IsChangeEmail = false;
            user.AwaitingConfirmEmail = null;
            user.EmailConfirmationCodeExpireTime = null;
            return await _userManager.UpdateAsync(user);
        }


    }
}