using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Extensions.Framework;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class SubscriptionRequestModel : HaveTenantIdCompanyIdRequestModelBase<Subscription>
    {
        public SubscriptionRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Subscription, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Package.ToString().Contains(Keyword) || x.Edition.Name.Contains(Keyword);
            }
            if (!string.IsNullOrWhiteSpace(TenantId))
            {
                ExpressionObj = ExpressionObj.And(x => x.TenantId == TenantId);
            }

            return ExpressionObj;
        }
    }
}
