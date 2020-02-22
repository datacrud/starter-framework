using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNet.Identity;
using Project.Core.Enums.Framework;
using Project.Core.Extensions.Framework;
using Project.Core.Handlers;
using Project.Core.StaticResource;
using Security.Models.Models;
using Security.Models.ViewModels;
using Security.Server.Managers;
using Security.Server.Repository;

namespace Security.Server.Service
{
    public class RoleService : SecurityServiceBase<Role>, IRoleService
    {
        private readonly RoleManager _roleManager;
        
        public RoleService(ISecurityRepository<Role, string> repository,
            RoleManager roleManager) : base(repository)
        {
            _roleManager = roleManager;
        }

        public override List<Role> GetAll()
        {
            return _roleManager.GetRoles();
        }


        public override Role GetById(object id)
        {
            string roleId = id as string;

            var role = _roleManager.FindById(roleId);

            return role;
        }

        private void CheckValidation(Role entity)
        {
            var isRoleExist = _roleManager.Roles().FilterTenant().Any(x => x.DisplayName == entity.DisplayName && x.Id != entity.Id);
            if (isRoleExist) throw new UiFriendlyException(HttpStatusCode.BadRequest, "Validation Failed!", $"Role {entity.DisplayName} already exist.");
        }

        public override Role Create(Role entity)
        {
            CheckValidation(entity);
            return _roleManager.Create(entity);
        }


        public override Role Update(Role entity)
        {
            CheckValidation(entity);
            return _roleManager.Update(entity);
        }

        public override Role Delete(object id)
        {
            string roleId = id as string;

            var role = _roleManager.FindById(roleId);

            if (UserManager.Users.Any(x => x.Roles.Any(y => y.RoleId == (string) id)))
            {
                throw new UiFriendlyException(HttpStatusCode.BadGateway, "Failed!",
                    "This role has data associate with users. Please remove the users first then try again");
            }

            _roleManager.Delete(role);

            return role;
        }

        public List<RoleViewModel> GetTenantRoles(string tenantId)
        {
            var roles = _roleManager.GetRoles(tenantId).ToList();

            return roles.ConvertAll(x => new RoleViewModel(x));
        }
    }
}