using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Core.DomainBase;

namespace Security.Models.Models
{
    public class User : IdentityUser<string, UserLogin, UserRole, UserClaim>, IEntity<string>, IHaveTenant<string>, IHaveCompany<string>
    {

        #region Tenant Properties

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

        #endregion


        public string TenantName { get; set; }
        public string BranchId { get; set; }
        public string EmployeeId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        
        public bool IsChangePassword { get; set; }
        public string PasswordChangeConfirmationCode { get; set; }
        public DateTime? PasswordChangeConfirmationCodeExpireTime { get; set; }

        public bool IsChangePhone { get; set; }
        public string AwaitingConfirmPhone { get; set; }
        public string PhoneConfirmationCode { get; set; }
        public DateTime? PhoneConfirmationCodeExpireTime { get; set; }



        public bool IsChangeEmail { get; set; }
        public string AwaitingConfirmEmail { get; set; }
        public string EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpireTime { get; set; }


        //future plan
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsShouldChangedPasswordOnNextLogin { get; set; }
        public bool IsEnablePasswordExpiration { get; set; }
        public bool IsPasswordExpired { get; set; }

        public DateTime? LastLoginTime { get; set; }


        //end future plan


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, string> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            if (!string.IsNullOrWhiteSpace(BranchId)) userIdentity.AddClaim(new Claim("BranchId", BranchId));
            if (!string.IsNullOrWhiteSpace(CompanyId)) userIdentity.AddClaim(new Claim("CompanyId", CompanyId));
            if (!string.IsNullOrWhiteSpace(TenantId)) userIdentity.AddClaim(new Claim("TenantId", TenantId));
            if (!string.IsNullOrWhiteSpace(TenantName)) userIdentity.AddClaim(new Claim("TenantName", TenantName));
            userIdentity.AddClaim(new Claim("UserId", Id));
            userIdentity.AddClaim(new Claim("UserName", UserName));
            if (!string.IsNullOrWhiteSpace(Email)) userIdentity.AddClaim(new Claim("UserEmail", Email));
            if(!string.IsNullOrWhiteSpace(EmployeeId)) userIdentity.AddClaim(new Claim("EmployeeId", EmployeeId));

            return userIdentity;
        }


        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}