using System.Collections.Generic;
using Security.Models.Models;
using Security.Models.ViewModels;

namespace Security.Server.Service
{
    public interface IRoleService :ISecurityServiceBase<Role>
    {
        List<RoleViewModel> GetTenantRoles(string tenantId);
    }
}