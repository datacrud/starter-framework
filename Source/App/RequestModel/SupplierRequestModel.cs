using System;
using System.Linq.Expressions;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class SupplierRequestModel : HaveTenantIdCompanyIdBranchIdRequestModelBase<Supplier>
    {
        public bool DueSupplierOnly { get; set; }

        public SupplierRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Supplier, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Name.Contains(Keyword) || x.Code.Contains(Keyword) || x.Phone.Contains(Keyword) || x.Email.Contains(Keyword);
            }

            if (DueSupplierOnly)
            {
                ExpressionObj = x => x.TotalDue > 0;
            }

            return ExpressionObj;
        }
    }
}