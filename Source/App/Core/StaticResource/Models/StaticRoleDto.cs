using Project.Core.Enums.Framework;
using Project.Core.Extensions;

namespace Project.Core.StaticResource.Models
{
    public class StaticRoleDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public AccessLevel AccessLevel { get; set; }

        public StaticRoleDto(string displayName, AccessLevel accessLevel = AccessLevel.Tenant)
        {
            Name = displayName.RemoveSpace();
            DisplayName = displayName;
            AccessLevel = accessLevel;
        }
    }
}