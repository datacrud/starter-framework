using System;
using System.Linq.Expressions;
using Project.Core.Extensions.Framework;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class CompanyRequestModel : HaveTenantIdRequestModelBase<Company>
    {
        public CompanyRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Company, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Name.Contains(Keyword) || x.Phone.Contains(Keyword);
            }

            if (!string.IsNullOrWhiteSpace(TenantId))
            {
                ExpressionObj = ExpressionObj.And(x => x.TenantId == TenantId);
            }

            return ExpressionObj;
        }

    }
}