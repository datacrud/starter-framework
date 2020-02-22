using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Project.Core.RequestModels;
using Project.ViewModel;
using Security.Models.RequestModels;
using Security.Models.ViewModels;

namespace Security.Server.Service
{
    public interface IProfileService 
    {
        ProfileViewModel GetUserProfile();
        Task<IdentityResult> UpdateProfile(UserProfileUpdateRequestModel model);
        bool ChangePassword(PasswordChangeRequestModel model);

        Task<EmailResponseModel> ConfirmEmail(EmailConfirmationRequestModel model);
        Task SendEmailConfirmationCode(EmailConfirmationRequestModel model);
        Task<bool> IsAlreadyExistAsync(string email);
        Task<IdentityResult> ResetProfile(string id);
    }
}