using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Core.Enums.Framework;
using Project.Core.Extensions.Framework;
using Project.Core.Session;
using Project.Core.StaticResource;
using Security.Models.Models;
using Security.Server.Repository;

namespace Security.Server.Managers
{


    public class RoleManager : RoleManager<Role, string>
    {
        private readonly IAppSession _appSession;
        private readonly RoleManager<Role> _roleManager;

        public RoleManager(ISecurityRepository<Role, string> repository) : base(new RoleStore<Role, string, UserRole>(new SecurityDbContext()))
        {
            _appSession = new AppSession();
            _roleManager = new RoleManager<Role>(new RoleStore<Role, string, UserRole>(new SecurityDbContext()));
        }



        public new IQueryable<Role> Roles()
        {
            return _roleManager.Roles.FilterTenant();
        }

        public List<Role> GetRoles(string tenantId = null, AccessLevel accessLevel = AccessLevel.Tenant)
        {
            if (string.IsNullOrWhiteSpace(tenantId)) tenantId = _appSession.TenantId;

            var roles = _roleManager.Roles.FilterTenant(tenantId).ToList();

            var isInRole = HttpContext.Current.User.IsInRole(StaticRoles.SystemAdmin.Name);
            if (isInRole) return roles;

            var systemAdminRole = roles.FirstOrDefault(role => role.Name == StaticRoles.SystemAdmin.Name);
            if (systemAdminRole != null) roles.Remove(systemAdminRole);

            var developerRole = roles.FirstOrDefault(role => role.Name == StaticRoles.Developer.Name);
            if (developerRole != null) roles.Remove(developerRole);

            isInRole = HttpContext.Current.User.IsInRole(StaticRoles.Admin.Name);
            if (isInRole) return roles;

            var adminRole = roles.FirstOrDefault(role => role.Name == StaticRoles.Admin.Name);
            if (adminRole != null) roles.Remove(adminRole);

            return roles;
        }


        public Role GetById(string id)
        {
            return _roleManager.FindById(id);
        }
        public Role GetByName(string roleName, string tenantId = null)
        {
            var role = _roleManager.Roles.FirstOrDefault(x => x.TenantId == tenantId && x.Name.ToLower() == roleName.ToLower());

            return role;
        }


        public Role Create(Role entity)
        {
            entity.Id = Guid.NewGuid().ToString();

            entity.Name = entity.DisplayName;

            entity.TenantId = _appSession.TenantId;
            entity.CompanyId = _appSession.CompanyId;
            entity.Created = DateTime.Now;
            entity.CreatedBy = _appSession.UserId;

            _roleManager.Create(entity);

            return entity;
        }

        public Role Update(Role entity)
        {
            var role = _roleManager.FindById(entity.Id);
            role.DisplayName = entity.DisplayName;
            role.TenantId = _appSession.TenantId;
            role.CompanyId = _appSession.CompanyId;
            role.Modified = DateTime.Now;
            role.ModifiedBy = _appSession.UserId;

            _roleManager.Update(role);

            return entity;
        }

        public void Delete(string id)
        {
            var role = _roleManager.FindById(id);

            role.TenantId = _appSession.TenantId;
            role.CompanyId = _appSession.CompanyId;
            role.IsDeleted = true;
            role.Deleted = DateTime.Now;
            role.DeletedBy = _appSession.UserId;

            _roleManager.Update(role);
        }



    }
}