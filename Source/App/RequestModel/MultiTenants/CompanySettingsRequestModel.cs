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
    public class CompanySettingsRequestModel : HaveTenantIdCompanyIdRequestModelBase<CompanySetting>
    {
        public CompanySettingsRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<CompanySetting, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.CustomerInvoiceTermsAndConditions.Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}
