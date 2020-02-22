using System;
using System.Collections.Generic;
using Security.Models.Models;

namespace Security.Models.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel(User model)
        {
            Id = model.Id;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            UserName = model.UserName;
            BranchId = model.BranchId;
            Roles = model.Roles;
            LastLoginTime = model.LastLoginTime;
            IsChangeEmail = model.IsChangeEmail;
            AwaitingConfirmEmail = model.AwaitingConfirmEmail;
            ConfirmationCode = null;
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        public string BranchId { get; set; }

        public ICollection<UserRole> Roles { get; set; }
        public ICollection<string> RoleNames { get; set; }

        public string RoleName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool IsChangeEmail { get; set; }
        public string AwaitingConfirmEmail { get; set; }
        public bool IsAwaitingConfirmEmail => IsChangeEmail && !string.IsNullOrWhiteSpace(AwaitingConfirmEmail);
        public string ConfirmationCode { get; set; }
    }

}