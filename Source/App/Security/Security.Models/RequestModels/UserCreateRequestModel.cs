using System;
using System.ComponentModel.DataAnnotations;

namespace Security.Models.RequestModels
{
    public class UserCreateRequestModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string RetypePassword { get; set; }

        public string SecurityStamp { get; set; }

        [Required]
        public string RoleId { get; set; }

        public string EmailConfirmationCode { get; set; }
        public string PhoneNumberConfirmationCode { get; set; }

        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }


        public bool EmailConfirmed { get; set; }
        public bool ChangeEmailAddress { get; set; }
        public string AwaitingVerifyEmailAddress { get; set; }
        public string EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsShouldChangedPasswordOnNextLogin { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public bool SendActivationEmailToUser { get; set; }
        public string Domain { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}