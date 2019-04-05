using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Model;

namespace Project.RequestModel
{
    public abstract class BaseRequestModel<TModel> where TModel : class 
    {
        public List<string> Tables { get; set; }
        public string Id { get; set; }

        protected Expression<Func<TModel, bool>> ExpressionObj = e => true;

        protected abstract Expression<Func<TModel, bool>> GetExpression();
    }
}
