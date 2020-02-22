using System.Collections.Generic;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Models.ViewModels;

namespace Security.Server.Service
{
    public interface IPermissionService : ISecurityServiceBase<Permission>
    {              
        bool CheckPermission(PermissionRequestModel model);
        List<Permission> GetAllByRoleId(string roleId);
        bool AddList(List<Permission> models);
        List<PermissionTreeViewModel> GetPermissionTree(string roleId);
    }
}