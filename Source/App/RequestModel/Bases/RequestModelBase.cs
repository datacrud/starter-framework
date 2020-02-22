using System;
using System.Linq;
using System.Linq.Expressions;
using Project.Core.DomainBase;
using Project.Model;

namespace Project.RequestModel.Bases
{
    public abstract class RequestModelBase<TModel> where TModel : Entity<string>, IHaveIsActive, IMayHaveOrder //BusinessEntityBase
    {

        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string IsAscending { get; set; }
        public OrderByRequest Request { get; }
        public int Page { get; set; }
        public int PageCount { get; set; }

        public DateTime? Date { get; set; }
        public string TenantId { get; set; }
        public string BranchId { get; set; }


        protected RequestModelBase(string keyword, string orderBy, string isAscending)
        {
            if (string.IsNullOrEmpty(keyword)) keyword = "";
            Keyword = keyword.ToLower();

            if (!string.IsNullOrWhiteSpace(orderBy)) OrderBy = orderBy;
            if (!string.IsNullOrWhiteSpace(isAscending)) IsAscending = isAscending;
            Request = new OrderByRequest
            {
                PropertyName = string.IsNullOrWhiteSpace(OrderBy) ? "Modified" : OrderBy,
                IsAscending = isAscending == "True"
            };

            Page = 1;
            PageCount = 10;
        }

        protected Expression<Func<TModel, bool>> ExpressionObj = model => true;

        public abstract Expression<Func<TModel, bool>> GetExpression();


        public Func<IQueryable<TSource>, IOrderedQueryable<TSource>> OrderByExpression<TSource>()
        {
            var propertyName = Request.PropertyName;
            var ascending = Request.IsAscending;
            var source = Expression.Parameter(typeof(IQueryable<TSource>), "source");
            var item = Expression.Parameter(typeof(TSource), "item");
            var member = Expression.Property(item, propertyName);
            var selector = Expression.Quote(Expression.Lambda(member, item));
            var body = Expression.Call(
                typeof(Queryable), ascending ? "OrderBy" : "OrderByDescending",
                new[] { item.Type, member.Type },
                source, selector);
            var lambda = Expression.Lambda<Func<IQueryable<TSource>, IOrderedQueryable<TSource>>>(body, source);
            var expression = lambda.Compile();
            return expression;
        }

        public IQueryable<TModel> GetSkipAndTakeQuery(IQueryable<TModel> queryable)
        {
            if (Page != -1)
            {
                queryable = queryable.Skip((Page - 1) * PageCount).Take(PageCount);
            }

            return queryable;
        }


        public IQueryable<TModel> GetOrderedDataQuery(IQueryable<TModel> queryable)
        {
            queryable = queryable.Where(GetExpression());
            queryable = OrderByExpression<TModel>()(queryable);
            return queryable;
        }

        public class OrderByRequest
        {
            public string PropertyName { get; set; }
            public bool IsAscending { get; set; }
        }
        
    }
}