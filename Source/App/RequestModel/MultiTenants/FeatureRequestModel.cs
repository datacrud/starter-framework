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
    public class FeatureRequestModel : RequestModelBase<Feature>
    {
        public FeatureRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Feature, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Name.Contains(Keyword) || x.ValueType.ToString().Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}
