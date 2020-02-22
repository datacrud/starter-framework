using System;
using System.Linq.Expressions;
using Project.Core.Extensions;
using Project.Core.Session;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class LoginAttemptRequestModel : HaveTenantIdCompanyIdBranchIdRequestModelBase<LoginAttempt>
    {
        public string Username { get; set; }

        public LoginAttemptRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
            ExpressionObj = base.GetExpression();
        }


        public override Expression<Func<LoginAttempt, bool>> GetExpression()
        {
            AppSession appSession = new AppSession();
            Username = appSession.UserName;

            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Username.Contains(Keyword) || x.Status.GetDescription().Contains(Keyword);
            }

            if (!string.IsNullOrWhiteSpace(Username))
            {
                ExpressionObj = x => x.Username.ToLower() == Username.ToLower();
            }


            return ExpressionObj;
        }

    }
}