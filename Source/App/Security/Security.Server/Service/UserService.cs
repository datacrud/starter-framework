using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.Core.Email;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Core.Helpers;
using Project.Core.Session;
using Project.Core.StaticResource;
using Project.Model;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Models.ViewModels;
using Security.Server.Managers;
using Security.Server.Providers;
using Security.Server.Repository;
using Security.Server.Stores;

namespace Security.Server.Service
{
    public class UserService : SecurityServiceBase<User>, IUserService
    {
        private readonly ISecurityRepository<User, string> _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly ITenantProvider _tenantProvider;
        private readonly IAppSession _appSession;
        private readonly RoleManager _roleManager;

        public UserService(ISecurityRepository<User, string> userRepository,
            IEmailSender emailSender,
            ITenantProvider tenantProvider,
            IAppSession appSession,
            RoleManager roleManager) : base(userRepository)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _tenantProvider = tenantProvider;
            _appSession = appSession;
            _roleManager = roleManager;
        }



        public List<User> GetUsers(AccessLevel accessLevel)
        {
            List<User> users;

            var isInSystemAdminRole = HttpContext.Current.User.IsInRole(StaticRoles.SystemAdmin.Name);
            if (isInSystemAdminRole)
            {
                users = GetUsers().OrderBy(x => x.TenantName).ToList();
                return users;
            }

            users = GetTenantUsers().ToList();

            var roles = _roleManager.Roles();
            var systemAdminRole = roles.First(x => x.Name == StaticRoles.SystemAdmin.Name);

            var systemAdminRoleUsers = GetTenantUsersByRole(systemAdminRole.Id).ToList();

            foreach (var user in systemAdminRoleUsers)
            {
                users.Remove(user);
            }

            return users;
        }

        public List<User> GetUsersByTenantId(string tenantId)
        {
            return GetUsers().Where(x => x.TenantId == tenantId && !x.IsDeleted).ToList();
        }

        public List<User> GetTenantUsersByTenantId(string tenantId)
        {
            var users = GetUsers().Where(x => x.TenantId == tenantId && !x.IsDeleted).ToList();

            var isInSystemAdminRole = HttpContext.Current != null && HttpContext.Current.User.IsInRole(StaticRoles.SystemAdmin.Name);
            if (isInSystemAdminRole)
            {
                return users;
            }

            var roles = _roleManager.Roles();
            var systemAdminRole = roles.First(x => x.Name == StaticRoles.SystemAdmin.Name);

            var systemAdminRoleUsers = GetTenantUsersByRole(systemAdminRole.Id, tenantId).ToList();

            foreach (var user in systemAdminRoleUsers)
            {
                users.Remove(user);
            }

            return users;
        }



        public async Task<List<User>> GetUsersByTenantIdAsync(string tenantId)
        {
            var users = await GetUsers().Where(x => x.TenantId == tenantId && !x.IsDeleted).ToListAsync();

            var isInSystemAdminRole = HttpContext.Current != null && HttpContext.Current.User.IsInRole(StaticRoles.SystemAdmin.Name);
            if (isInSystemAdminRole)
            {
                return users;
            }

            var roles = _roleManager.Roles();
            var systemAdminRole = roles.FirstOrDefault(x => x.Name == StaticRoles.SystemAdmin.Name);

            if (systemAdminRole != null)
            {
                var systemAdminRoleUsers = GetTenantUsersByRole(systemAdminRole.Id, tenantId).ToList();

                foreach (var user in systemAdminRoleUsers)
                {
                    users.Remove(user);
                }
            }

            return users;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users;

            var isInSystemAdminRole = HttpContext.Current.User.IsInRole(StaticRoles.SystemAdmin.Name);
            if (isInSystemAdminRole)
            {
                users = await GetUsers().OrderBy(x => x.TenantName).ToListAsync();
                return users;
            }

            users = GetTenantUsers().ToList();

            var roles = _roleManager.Roles();
            var systemAdminRole = roles.First(x => x.Name == StaticRoles.SystemAdmin.Name);

            var systemAdminRoleUsers = GetTenantUsersByRole(systemAdminRole.Id).ToList();

            foreach (var user in systemAdminRoleUsers)
            {
                users.Remove(user);
            }

            return users;
        }


        public async Task<IdentityResult> CreateUserAsync(UserCreateRequestModel model)
        {
            if (model.PasswordHash != model.RetypePassword) return new IdentityResult("Password does not match!");

            string id = model.Id;
            if (string.IsNullOrWhiteSpace(model.Id)) id = Guid.NewGuid().ToString();

            var applicationUser = GetReadyUserForCreate(model, id);

            return await CreateUserAsync(applicationUser);
        }


        public async Task<IdentityResult> UpdateUserAsync(UserCreateRequestModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);

            var oldRoleId = user.Roles.FirstOrDefault()?.RoleId;
            if (oldRoleId != model.RoleId) await UpdateUserRoleAsync(user.Id, oldRoleId, model.RoleId);

            if (model.PasswordHash != model.RetypePassword) return await UpdateUserAsync(user);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrEmpty(model.PasswordHash))
                user.PasswordHash = new PasswordHasher().HashPassword(model.PasswordHash);

            user.TenantId = model.TenantId;
            user.TenantName = model.TenantName;
            user.CompanyId = model.CompanyId;
            user.BranchId = model.BranchId;


            user.IsChangeEmail = model.ChangeEmailAddress;
            user.AwaitingConfirmEmail = model.AwaitingVerifyEmailAddress;
            user.EmployeeId = model.EmployeeId;
            user.IsActive = model.IsActive;
            user.IsShouldChangedPasswordOnNextLogin = model.IsShouldChangedPasswordOnNextLogin;
            user.Created = model.Created.GetValueOrDefault();
            user.CreatedBy = model.CreatedBy;
            user.Modified = model.Modified;
            user.ModifiedBy = model.ModifiedBy;

            if (!user.Email.IsNullOrWhiteSpace() && !user.UserName.IsReservedUsername()) user.UserName = user.Email;

            return await UpdateUserAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await UserManager.UpdateAsync(user);
        }


        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            user.IsDeleted = true;
            user.IsActive = false;
            user.UserName = user.UserName + "_deleted@" + DateTime.Now.Ticks;
            user.Email = "deleted" + DateTime.Now.Ticks + "_" + user.Email;
            return await UpdateUserAsync(user);

            //return  await DeleteUserAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            user.IsDeleted = true;
            user.IsActive = false;
            user.UserName = user.UserName + "_deleted@" + DateTime.Now.Ticks;
            user.Email = "deleted" + DateTime.Now.Ticks + "_" + user.Email;
            return await UpdateUserAsync(user);

            //return await DeleteUserAsync(user);
        }

        public IdentityResult CreateTenantAdminUser(UserCreateRequestModel model)
        {
            if (model.PasswordHash != model.RetypePassword) return null;

            string id = model.Id;
            if (string.IsNullOrWhiteSpace(model.Id)) id = Guid.NewGuid().ToString();
            User user = GetReadyUserForCreate(model, id);

            var applicationUser = user;

            return CreateTenantAdminUser(applicationUser);
        }

        public async Task<bool> IsEmailVerificationCodeExist(EmailVerificationRequestModel model)
        {
            var user = await UserManager.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == model.UserId);

            return !string.IsNullOrWhiteSpace(user?.EmailConfirmationCode);
        }

        public async Task<IdentityResult> ConfirmedEmailAsync(string userId, string code)
        {
            //await ConfirmEmailAsync(userId, code);

            var user = await UserManager.FindByIdAsync(userId);
            user.EmailConfirmationCode = null;
            user.EmailConfirmed = true;

            user.IsActive = true;

            return await UserManager.UpdateAsync(user);
        }

        public Task<User> GetUserAsNoTrackingAsync(string userId)
        {
            return UserManager.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId);
        }


        public Task<User> FindByEmailAsync(string email)
        {
            return UserManager.FindByEmailAsync(email);
        }

        public async Task<List<User>> GetFilterUser(string tenantId)
        {
            return await _userRepository.GetAllIgnoreFilter().Where(x => x.TenantId == tenantId).ToListAsync();
        }





        public async Task SendEmailConfirmationLinkAsync(string userId, string fullName, string email, string emailCode)
        {

            var subject = "Confirm Your Account";
            var body = "This email has been send to you to confirm your account. Please click the link bellow to confirm your account. <br/><br/>";
            body += "<strong>Confirmation Link: </strong>" + EmailHelper.EmailVerificationUri() + "?code=" + emailCode + "&uid=" + userId;

            await _emailSender.SendSecurityEmailAsync(email, fullName, subject, body);
        }

        public async Task ResendEmailVerificationCodeAsync(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null && !user.EmailConfirmed)
            {
                user.EmailConfirmed = false;
                user.EmailConfirmationCode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                await UserManager.UpdateAsync(user);

                await SendEmailConfirmationLinkAsync(user.Id, user.FullName(), user.Email, user.EmailConfirmationCode);
            }
        }

        public async Task SendPasswordResetLinkAsync(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                var passwordResetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                user.PasswordChangeConfirmationCode = passwordResetToken;
                await UserManager.UpdateAsync(user);

                var subject = "Reset Your Account";
                var body = "This email has been send to you reset your password. Please click the link bellow to reset your password: <br/><br/>";
                body += "<strong>Reset Password Link: </strong>" + EmailHelper.ResetPasswordUri() + "?code=" + passwordResetToken + "&uid=" + user.Id;

                await _emailSender.SendSecurityEmailAsync(email, user.FullName(), subject, body);
            }
        }

        public async Task<bool> IsPasswordResetCodeExist(string code, string userId)
        {
            var user = await UserManager.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId);

            return !string.IsNullOrWhiteSpace(user?.PasswordChangeConfirmationCode);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string code, string password)
        {
            //return await ResetPasswordAsync(userId, code, password);

            var user = await UserManager.FindByIdAsync(userId);
            var hashPassword = new PasswordHasher().HashPassword(password);
            user.PasswordHash = hashPassword;

            user.PasswordChangeConfirmationCode = null;

            user.IsActive = true;
            user.IsLocked = false;

            return await UserManager.UpdateAsync(user);
        }





        public bool IsReachedMaximumUsersCount(string tenantId, int featureMaximumAllowedUsers)
        {
            if (featureMaximumAllowedUsers == 0) return false;

            var users = GetTenantUsersByTenantId(tenantId);

            var tenant = _tenantProvider.GetAll().AsNoTracking().FirstOrDefault(x => x.Id == tenantId);
            //var showroom = tenant?.NoOfShowroom ?? 0;
            var bonusUser = tenant?.BonusUserAccessCount ?? 0;

            //return users.Count >= (featureMaximumAllowedUsers * showroom) + bonusUser;
            return users.Count >= (featureMaximumAllowedUsers) + bonusUser;
        }

        public bool IsLastUser(string userId)
        {
            var user = GetUsers().AsNoTracking().FirstOrDefault(x => x.Id == userId);
            if (user == null) return false;

            var count = GetTenantUsers(user.TenantId).AsNoTracking().Count();
            if (count == 1)
                return true;

            return false;
        }





        public List<UserAlertViewModel> CheckUserAlert(string tenantId)
        {
            List<UserAlertViewModel> viewModels = new List<UserAlertViewModel>();

            var users = GetUsersByTenantId(tenantId);
            var usersCount = users.Count;
            if (usersCount == 1)
            {
                var message = $"Currently you have only {usersCount} user access. You can create more user access from here " +
                              $"<a class='btn btn-xs blue' href='#!/user'>Create New</a>";

                viewModels.Add(new UserAlertViewModel()
                {
                    IsShow = true,
                    Identity = 1,
                    Title = "User Access: ",
                    AlertType = DashboardAlertType.Info,
                    AlertClass = CssHelpers.Info,
                    Message =
                        message
                });
            }

            return viewModels;
        }

        public async Task<IdentityResult> CreateTenantAdminUserAsync(UserCreateRequestModel model)
        {
            if (model.PasswordHash != model.RetypePassword) return null;

            string id = model.Id;
            if (string.IsNullOrWhiteSpace(model.Id)) id = Guid.NewGuid().ToString();
            User user = GetReadyUserForCreate(model, id);

            var applicationUser = user;

            return await CreateTenantAdminUserAsync(applicationUser);
        }

        private static User GetReadyUserForCreate(UserCreateRequestModel model, string id)
        {
            return new User()
            {
                Id = id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                PasswordHash = new PasswordHasher().HashPassword(model.PasswordHash),
                SecurityStamp = Guid.NewGuid().ToString(),
                Roles = { new UserRole() { RoleId = model?.RoleId, UserId = id, TenantId = model?.TenantId, CompanyId = model?.CompanyId } },

                EmailConfirmed = model.EmailConfirmed,
                EmailConfirmationCode = model.EmailConfirmationCode,
                PhoneConfirmationCode = model.PhoneNumberConfirmationCode,

                TenantId = model.TenantId,
                TenantName = model.TenantName,
                CompanyId = model.CompanyId,
                BranchId = model.BranchId,


                IsChangeEmail = model.ChangeEmailAddress,
                AwaitingConfirmEmail = model.AwaitingVerifyEmailAddress,
                EmployeeId = model.EmployeeId,
                IsActive = model.IsActive,
                IsShouldChangedPasswordOnNextLogin = model.IsShouldChangedPasswordOnNextLogin,
                Created = DateTime.Now,
                CreatedBy = model.CreatedBy,
                //Modified= model.Modified,
                //ModifiedBy= model.ModifiedBy,

            };
        }

        public List<User> GetAll(AccessLevel accessLevel)
        {
            throw new NotImplementedException();
        }

        public User GetById(string id)
        {
            throw new NotImplementedException();
        }


        public User Delete(string id)
        {
            throw new NotImplementedException();
        }



        public bool IsEmailExist(string email)
        {
            return _userRepository.GetAll().AsNoTracking().Any(x => x.Email == email && !x.IsDeleted);
        }

        public IQueryable<User> GetUsers()
        {
            return UserManager.Users.Include(x => x.Roles).AsQueryable();
        }

        public IQueryable<User> GetTenantUsers()
        {
            return UserManager.Users.Where(x => x.TenantId == _appSession.TenantId).Include(x => x.Roles).AsQueryable();
        }

        public IQueryable<User> GetTenantUsers(string tenantId)
        {
            return UserManager.Users.Where(x => x.TenantId == tenantId).Include(x => x.Roles).AsQueryable();
        }

        public IQueryable<User> GetTenantUsersByRole(string roleId, string tenantId = null)
        {
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                tenantId = _appSession.TenantId;
            }

            return UserManager.Users.Where(x => x.TenantId == tenantId)
                .Where(x => !x.IsDeleted && x.Roles.Select(y => y.RoleId).Contains(roleId)).AsQueryable();
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await UserManager.FindByIdAsync(id);
        }


        public async Task<IdentityResult> CreateUserAsync(User user)
        {
            return await UserManager.CreateAsync(user);
        }



        public IdentityResult CreateTenantAdminUser(User user)
        {
            var store = new AppUserStore(new BusinessDbContext());
            var userManager = new UserManager(store);
            userManager.SetTenantId(user.TenantId);

            var identityResult = userManager.Create(user);

            return identityResult;
        }


        public async Task UpdateUserRoleAsync(string userId, string oldRoleId, string newRoleId)
        {
            var oldRole = _roleManager.FindById(oldRoleId);
            var newRole = _roleManager.FindById(newRoleId);

            if (oldRole != null) await UserManager.RemoveFromRoleAsync(userId, oldRole.Name);

            await UserManager.AddRoleToUserAsync(userId, newRole.Name, _appSession.TenantId);
        }

        public async Task<IdentityResult> CreateTenantAdminUserAsync(User user)
        {
            var store = new AppUserStore(new SecurityDbContext());
            var userManager = new UserManager(store);
            userManager.SetTenantId(user.TenantId);

            var task = await userManager.CreateAsync(user);

            return task;
        }
    }
}