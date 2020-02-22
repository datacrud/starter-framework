using System;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class LoginAttempt : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        public DateTime? Date { get; set; }
        public string Username { get; set; }
        public LoginAttemptStatus Status { get; set; }
        public string Error { get; set; }
    }
}