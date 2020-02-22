using System;
using System.Linq.Expressions;
using Project.Core.Enums;
using Project.Core.Extensions.Framework;
using Project.Model;
using Project.RequestModel.Bases;

namespace Project.RequestModel
{
    public class CustomerRequestModel : HaveTenantIdCompanyIdBranchIdRequestModelBase<Customer>
    {
        public CustomerType Type { get; set; }
        public bool DueCustomerOnly { get; set; }

        public CustomerRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
            Type = 0;
        }        

        public override Expression<Func<Customer, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj =
                    x =>
                        x.Name.Contains(Keyword) || x.Code.Contains(Keyword) || x.Phone.Contains(Keyword) ||
                        x.Email.Contains(Keyword);
            }

            if (Type != CustomerType.All && !string.IsNullOrWhiteSpace(Type.ToString()))
            {
                ExpressionObj = ExpressionObj.And(x => x.Type == Type);
            }

            if (DueCustomerOnly)
            {
                ExpressionObj = ExpressionObj.And(x => x.TotalDue > 0);
            }

            return ExpressionObj;
        }


    }
}