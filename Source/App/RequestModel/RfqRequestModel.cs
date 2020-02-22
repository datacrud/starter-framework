using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class RfqRequestModel : RequestModelBase<Rfq>
    {
        public RfqRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Rfq, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.CompanyName.Contains(Keyword) || x.YourName.Contains(Keyword) ||
                    x.EmailAddress.Contains(Keyword) || x.PhoneNumber.Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}
