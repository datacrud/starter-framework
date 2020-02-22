using System;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class LoginAttemptViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<LoginAttempt>
    {
        public LoginAttemptViewModel(LoginAttempt model) : base(model)
        {
            Date = model.Date;
            Username = model.Username;
            Status = model.Status;
            Error = model.Error;
        }

        public DateTime? Date { get; set; }
        public string Username { get; set; }
        public LoginAttemptStatus Status { get; set; }
        public string StatusName => Status.GetDescription();
        public string Error { get; set; }

    }
}