using System;
using System.Linq;
using System.Linq.Expressions;
using Project.Core.Helpers.Framework;

namespace Project.Core.Extensions.Framework
{
    
    public static class ExpressionExtension
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ExpressionParameterRewriter.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static IQueryable<T> WhereIn<T, TV>(this IQueryable<T> source, Expression<Func<T, TV>> valueSelector, params TV[] values)
        {
            var condition = Expression.Call(
                typeof(Enumerable), "Contains", new[] { typeof(TV) },
                Expression.Constant(values), valueSelector.Body);
            var predicate = Expression.Lambda<Func<T, bool>>(condition, valueSelector.Parameters);
            return source.Where(predicate);
        }

        public static IQueryable<TEntity> ApplyOrder<TEntity>(this IQueryable<TEntity> queryable, string propertyName, bool isAscending = true)
        {
            var propertyInfo = typeof(IQueryable<TEntity>).GetProperty(propertyName);

            return isAscending
                ? queryable.OrderBy(x => propertyInfo.GetValue(x, null))
                : queryable.OrderByDescending(x => propertyInfo.GetValue(x, null));
        }

    }
}