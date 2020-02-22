using System;
using System.Linq.Expressions;
using Project.Core.Extensions.Framework;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.RequestModel.Bases
{
    public abstract class HaveTenantIdCompanyIdBranchIdRequestModelBase<TModel> : RequestModelBase<TModel> where TModel : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        protected HaveTenantIdCompanyIdBranchIdRequestModelBase(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
            ExpressionObj = PrepareTenantExpressionBase();
        }

        private Expression<Func<TModel, bool>> PrepareTenantExpressionBase()
        {
            if (!string.IsNullOrWhiteSpace(TenantId))
            {
                ExpressionObj = ExpressionObj.And(x => x.TenantId == TenantId);
            }

            if (!string.IsNullOrWhiteSpace(BranchId))
            {
                ExpressionObj = ExpressionObj.And(x => x.BranchId == BranchId);
            }

            return ExpressionObj;
        }

        public override Expression<Func<TModel, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(BranchId))
            {
                ExpressionObj = ExpressionObj.And(x => x.BranchId == BranchId);
            }

            if (!string.IsNullOrWhiteSpace(TenantId))
            {
                ExpressionObj = ExpressionObj.And(x => x.TenantId == TenantId);
            }

            return ExpressionObj;
        }




    }
}