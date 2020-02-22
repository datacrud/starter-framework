using Project.Model.EntityBase;

namespace Project.RequestModel.Bases
{
    public abstract class HaveTenantIdCompanyIdRequestModelBase<TModel> : RequestModelBase<TModel> where TModel : HaveTenantIdCompanyIdEntityBase
    {
        protected HaveTenantIdCompanyIdRequestModelBase(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }
    }
}