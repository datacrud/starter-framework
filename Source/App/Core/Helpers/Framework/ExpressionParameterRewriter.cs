using System.Collections.Generic;
using System.Linq.Expressions;

namespace Project.Core.Helpers.Framework
{
    public class ExpressionParameterRewriter : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ExpressionParameterRewriter(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ExpressionParameterRewriter(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (map.TryGetValue(p, out var replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }


    //public class ReplaceExpressionVisitor : ExpressionVisitor
    //{
    //    private readonly Expression _newValue;
    //    private readonly Expression _oldValue;

    //    public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
    //    {
    //        _oldValue = oldValue;
    //        _newValue = newValue;
    //    }

    //    public override Expression Visit(Expression node)
    //    {
    //        if (node == _oldValue)
    //            return _newValue;
    //        return base.Visit(node);
    //    }
    //}
}