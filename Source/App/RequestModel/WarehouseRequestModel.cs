using System;
using System.Linq.Expressions;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class WarehouseRequestModel : HaveTenantIdCompanyIdRequestModelBase<Warehouse>
    {
        public WarehouseRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Warehouse, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Name.Contains(Keyword) || x.Code.Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}