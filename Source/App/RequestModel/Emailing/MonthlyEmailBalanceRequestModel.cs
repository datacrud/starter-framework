using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Extensions;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class MonthlyEmailBalanceRequestModel : HaveTenantIdCompanyIdBranchIdRequestModelBase<MonthlyEmailBalance>
    {
        public MonthlyEmailBalanceRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<MonthlyEmailBalance, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Month.GetDescription().Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}
