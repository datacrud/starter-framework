using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Project.Core.Enums.Framework;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Models.ViewModels;

namespace Security.Server.Service
{
    public interface IUserService : ISecurityServiceBase<User>
    {
        List<User> GetAll(AccessLevel accessLevel);
        User GetById(string id);
        User Delete(string id);


        List<User> GetUsers(AccessLevel accessLevel);
        List<User> GetUsersByTenantId(string tenantId);

        Task<User> GetUserAsync(string id);
        Task<List<User>> GetUsersByTenantIdAsync(string tenantId);
        Task<List<User>> GetUsersAsync();

        Task<IdentityResult> CreateUserAsync(UserCreateRequestModel model);
        Task<IdentityResult> CreateTenantAdminUserAsync(UserCreateRequestModel model);
        Task<IdentityResult> UpdateUserAsync(UserCreateRequestModel model);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<IdentityResult> DeleteUserAsync(User user);

        IdentityResult CreateTenantAdminUser(UserCreateRequestModel adminUser);

        Task<bool> IsEmailVerificationCodeExist(EmailVerificationRequestModel model);
        Task<IdentityResult> ConfirmedEmailAsync(string userId, string code);
        Task<User> GetUserAsNoTrackingAsync(string userId);
        Task<bool> IsPasswordResetCodeExist(string code, string userId);
        Task<IdentityResult> ResetPasswordAsync(string userId, string code, string password);
        Task<User> FindByEmailAsync(string email);
        Task<List<User>> GetFilterUser(string tenantId);

        bool IsReachedMaximumUsersCount(string tenantId, int featureMaximumAllowedUsers);
        bool IsLastUser(string userId);


        Task SendEmailConfirmationLinkAsync(string userId, string fullName, string email, string emailCode);
        Task ResendEmailVerificationCodeAsync(string email);
        Task SendPasswordResetLinkAsync(string email);

        List<UserAlertViewModel> CheckUserAlert(string tenantId);




        bool IsEmailExist(string email);
        IQueryable<User> GetUsers();
        IQueryable<User> GetTenantUsers();
        IQueryable<User> GetTenantUsers(string tenantId);
        IQueryable<User> GetTenantUsersByRole(string roleId, string tenantId = null);


        Task<IdentityResult> CreateUserAsync(User user);
        Task<IdentityResult> CreateTenantAdminUserAsync(User user);
        Task UpdateUserRoleAsync(string userId, string oldRoleId, string newRoleId);

        IdentityResult CreateTenantAdminUser(User user);
    }
}