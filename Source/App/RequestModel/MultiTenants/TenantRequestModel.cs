using System;
using System.Linq.Expressions;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class TenantRequestModel : RequestModelBase<Tenant>
    {
        public TenantRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Tenant, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.Name.Contains(Keyword) || x.TenancyName.Contains(Keyword) || x.Edition.Name.Contains(Keyword) ||
                    x.Edition.DisplayName.Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}