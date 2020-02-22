using System;
using System.Linq.Expressions;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class FiscalYearRequestModel : HaveTenantIdCompanyIdBranchIdRequestModelBase<FiscalYear>
    {
        public FiscalYearRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public DateTime Date { get; set; } = DateTime.Today;

        public override Expression<Func<FiscalYear, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.StartDate.GetValueOrDefault().Date >= Date && x.EndDate.GetValueOrDefault().Date <= Date;
            }


            return ExpressionObj;
        }
    }
}