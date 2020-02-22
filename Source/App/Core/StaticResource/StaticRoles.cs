using System.Collections.Generic;
using Project.Core.Enums.Framework;
using Project.Core.StaticResource.Models;

namespace Project.Core.StaticResource
{
    public static class StaticRoles
    {
        public static StaticRoleDto SystemAdmin = new StaticRoleDto("System Admin", AccessLevel.Host);
        public static StaticRoleDto Developer = new StaticRoleDto("Developer", AccessLevel.Host);
        public static StaticRoleDto Support = new StaticRoleDto("Support", AccessLevel.Host);
        public static StaticRoleDto Admin = new StaticRoleDto("Admin");
        public static StaticRoleDto Manager = new StaticRoleDto("Manager");
        public static StaticRoleDto BranchAdmin = new StaticRoleDto("Branch Admin");
        public static StaticRoleDto BranchManager = new StaticRoleDto("Branch Manager");
        public static StaticRoleDto User = new StaticRoleDto("User");



        public static List<StaticRoleDto> GetRoles()
        {
            return new List<StaticRoleDto>
            {
                SystemAdmin,
                Developer,
                Admin,
                Manager,
                BranchAdmin,
                BranchManager,
                User
            };
        }
    }
}