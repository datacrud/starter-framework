using System;
using System.Linq.Expressions;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class PartnerRequestModel : HaveTenantIdCompanyIdBranchIdRequestModelBase<Partner>
    {
        public PartnerRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Partner, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Name.Contains(Keyword) || x.Type.ToString().Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}