using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.RequestModel.Bases
{
    public abstract class HaveTenantIdRequestModelBase<TModel> : RequestModelBase<TModel> where TModel : HaveTenantIdEntityBase
    {
        protected HaveTenantIdRequestModelBase(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }
    }
}