using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Project.Core.Handlers;
using Project.Core.Session;
using Project.Model;
using Security.Models.Models;
using Security.Server.Repository;
using Security.Server.Service;
using Security.Server.Stores;

namespace Security.Server.Managers
{
    public class UserManager : UserManager<User, string>
    {
        private readonly IAppSession _appSession;
        private readonly AppUserStore _userStore;

        public UserManager(AppUserStore store)
            : base(store)
        {
            _userStore = store;
            _appSession = new AppSession();
        }


        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var manager = new UserManager(new AppUserStore(context.Get<BusinessDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public void SetTenantId(string tenantId)
        {
            _userStore.SetTenantId(tenantId);
        }


        public override async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            var user = await _userStore.FindByIdAsync(userId);
            await _userStore.RemoveFromRoleAsync(user, role);

            return IdentityResult.Success;
        }

        public async Task AddRoleToUserAsync(User user, string roleName, string tenantId)
        {
            await _userStore.AddToRoleAsync(user, roleName, tenantId);
        }

        public async Task AddRoleToUserAsync(string userId, string roleName, string tenantId)
        {
            await _userStore.AddToRoleAsync(userId, roleName, tenantId);
        }

        public async Task AddRoleToUserAsync(string userId, string roleId, string tenantId, string companyId)
        {
            await _userStore.AddToRoleAsync(userId, roleId, tenantId, companyId);
        }


        public void AddRoleToUser(User user, string roleName, string tenantId)
        {
            _userStore.AddToRole(user, roleName, tenantId);
        }

        public void AddRoleToUser(string userId, string roleName, string tenantId)
        {
            _userStore.AddToRole(userId, roleName, tenantId);
        }

        public void AddRoleToUser(string userId, string roleId, string tenantId, string companyId)
        {
            _userStore.AddToRole(userId, roleId, tenantId, companyId);
        }




        public async Task<User> FindAsync(string tenantId, string username, string password)
        {
            var user = await _userStore.Users.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.UserName == username);
            if (user != null)
            {
                var result = PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);
                if (result == PasswordVerificationResult.Failed)
                {
                    throw new UiFriendlyException(HttpStatusCode.BadRequest, "Login Failed!", "Username or password could not match");
                }
            }

            if (user == null)
            {
                throw new UiFriendlyException(HttpStatusCode.BadRequest, "Login Failed!", "Username or password could not match");
            }

            return user;
        }
    }
}
