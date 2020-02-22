using Project.Core.Enums.Framework;
using Project.Model.EntityBases;
using Project.RequestModel.Bases;
using Project.ViewModel;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public interface IHaveTenantIdPagingService<T, TVm> where T : HaveTenantIdEntityBase where TVm : HaveTenantIdViewModelBase<T>
    {
        ResponseModel<TVm> GetAllAsTenant(HaveTenantIdRequestModelBase<T> requestModel);

        ResponseModel<TVm> SearchAsTenant(PagingDataType status, HaveTenantIdRequestModelBase<T> requestModel);
    }
}