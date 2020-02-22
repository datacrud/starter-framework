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
    public class AuditLogRequestModel : HaveTenantIdCompanyIdBranchIdRequestModelBase<AuditLog>
    {
        public AuditLogRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<AuditLog, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.EntityInvoiceNo.Contains(Keyword) || x.EntityType.ToString().Contains(Keyword) ||
                    x.ActionType.ToString().Contains(Keyword) || x.Data.Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}
