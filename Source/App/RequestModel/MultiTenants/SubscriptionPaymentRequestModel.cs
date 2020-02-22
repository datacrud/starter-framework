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
    public class SubscriptionPaymentRequestModel : HaveTenantIdCompanyIdRequestModelBase<SubscriptionPayment>
    {
        public SubscriptionPaymentRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }


        public override Expression<Func<SubscriptionPayment, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.TransectionNumber.Contains(Keyword) || x.VerificationCode.ToString().Contains(Keyword);
            }

            if (!string.IsNullOrWhiteSpace(TenantId))
            {
                ExpressionObj = ExpressionObj.And(x => x.TenantId == TenantId);
            }

            return ExpressionObj;
        }
    }
}
