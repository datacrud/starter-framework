using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Core.Handlers;
using Project.Model;
using Security.Models.Models;

namespace Security.Server.Stores
{
    public class AppUserStore : UserStore<User, Role, string, UserLogin, UserRole, UserClaim>
    {
        private bool IsSetTenantId { get; set; }

        private string TenantId { get; set; }

        public AppUserStore(SecurityDbContext dbContext) : base(dbContext) { }

        public void SetTenantId(string tenantId)
        {
            TenantId = tenantId;
            IsSetTenantId = true;
        }

        public override Task CreateAsync(User user)
        {
            if(!IsSetTenantId) throw new UiFriendlyException(HttpStatusCode.Forbidden, "Set Tenant Id", "Tenant ID is not set in user manager");

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            //if (string.IsNullOrWhiteSpace(TenantId) && string.IsNullOrWhiteSpace(user.TenantId))
            //{
            //    throw new ArgumentNullException(nameof(TenantId));
            //}

            user.TenantId = this.TenantId;

            return base.CreateAsync(user);
        }

        public override Task<User> FindByEmailAsync(string email)
        {
            if (!IsSetTenantId) throw new UiFriendlyException(HttpStatusCode.Forbidden, "Set Tenant Id", "Tenant ID is not set in user manager");

            //if (string.IsNullOrWhiteSpace(TenantId))
            //{
            //    throw new ArgumentNullException(nameof(TenantId));
            //}

            return this.GetUserAggregateAsync(u => u.Email.ToUpper() == email.ToUpper()
                                                   && u.TenantId == this.TenantId);
        }


        public override Task<User> FindByNameAsync(string userName)
        {
            if (!IsSetTenantId) throw new UiFriendlyException(HttpStatusCode.Forbidden, "Set Tenant Id", "Tenant ID is not set in user manager");

            //if (string.IsNullOrWhiteSpace(TenantId))
            //{
            //    throw new ArgumentNullException(nameof(TenantId));
            //}

            return this.GetUserAggregateAsync(u => u.UserName.ToUpper() == userName.ToUpper()
                                                   && u.TenantId == this.TenantId);
        }

        public override async Task RemoveFromRoleAsync(User user, string roleName)
        {
            var role = await Context.Set<Role>().AsNoTracking().SingleAsync(x => x.Name == roleName && x.TenantId == TenantId);

            var userRole = await Context.Set<UserRole>().SingleAsync(x => x.UserId == user.Id && x.RoleId == role.Id && x.TenantId == TenantId);

            Context.Set<UserRole>().Remove(userRole);
        }


        public Task AddToRoleAsync(User user, string roleName, string tenantId)
        {
            Role role = null;
            Company company = null;

            try
            {
                role = Context.Set<Role>().Single(mr => mr.Name == roleName && mr.TenantId == tenantId);
                company = Context.Set<Company>().Single(mr => mr.TenantId == tenantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Context.Set<UserRole>().Add(new UserRole
            {
                TenantId = tenantId,
                CompanyId = company?.Id,
                RoleId = role.Id,
                UserId = user.Id
            });

            return Context.SaveChangesAsync();
        }

        public void AddToRole(User user, string roleName, string tenantId)
        {
            Role role = null;
            Company company = null;

            try
            {
                role = Context.Set<Role>().Single(mr => mr.Name == roleName && mr.TenantId == tenantId);
                company = Context.Set<Company>().Single(mr => mr.TenantId == tenantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Context.Set<UserRole>().Add(new UserRole
            {
                TenantId = tenantId,
                CompanyId = company?.Id,
                RoleId = role.Id,
                UserId = user.Id
            });

            Context.SaveChangesAsync();
        }



        public Task AddToRoleAsync(string userId, string roleName, string tenantId)
        {
            Role role = null;
            Company company = null;

            try
            {
                role = Context.Set<Role>().Single(mr => mr.Name == roleName && mr.TenantId == tenantId);
                company = Context.Set<Company>().Single(mr => mr.TenantId == tenantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Context.Set<UserRole>().Add(new UserRole
            {
                TenantId = tenantId,
                CompanyId = company?.Id,
                RoleId = role.Id,
                UserId = userId
            });

            return Context.SaveChangesAsync();
        }

        public void AddToRole(string userId, string roleName, string tenantId)
        {
            Role role = null;
            Company company = null;

            try
            {
                role = Context.Set<Role>().Single(mr => mr.Name == roleName && mr.TenantId == tenantId);
                company = Context.Set<Company>().Single(mr => mr.TenantId == tenantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Context.Set<UserRole>().Add(new UserRole
            {
                TenantId = tenantId,
                CompanyId = company?.Id,
                RoleId = role.Id,
                UserId = userId
            });

            Context.SaveChanges();
        }



        public Task AddToRoleAsync(string userId, string roleId, string tenantId, string companyId)
        {
            Context.Set<UserRole>().Add(new UserRole
            {
                TenantId = tenantId,
                CompanyId = companyId,
                RoleId = roleId,
                UserId = userId
            });

            return Context.SaveChangesAsync();
        }

        public void AddToRole(string userId, string roleId, string tenantId, string companyId)
        {
            Context.Set<UserRole>().Add(new UserRole
            {
                TenantId = tenantId,
                CompanyId = companyId,
                RoleId = roleId,
                UserId = userId
            });

            Context.SaveChanges();
        }
    }
}