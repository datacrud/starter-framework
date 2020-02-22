using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Core.DomainBase;
using Project.Core.Enums.Framework;


namespace Security.Models.Models
{
    public class Role: IdentityRole<string, UserRole>, IEntity<string>, IHaveTenant<string>, IHaveCompany<string>
    {
        public string DisplayName { get; set; }

        public AccessLevel AccessLevel { get; set; } = AccessLevel.Tenant;
        public bool IsStatic { get; set; }

        public string TenantId { get; set; }
        public string CompanyId { get; set; }

        public DateTime Created { get; set; } = DateTime.Today;
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")] public virtual User Creator { get; set; }

        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")] public virtual User Modifier { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime? Deleted { get; set; }
        public string DeletedBy { get; set; }
        [ForeignKey("DeletedBy")] public virtual User Deleter { get; set; }

        public string DeviceInfo { get; set; }
        public string IpAddress { get; set; }

        public Role()
        {
            
        }

        public Role(string name, string displayName, AccessLevel accessLevel = AccessLevel.Tenant)
        {
            Name = name;
            DisplayName = displayName;
            AccessLevel = accessLevel;
        }
    }
}