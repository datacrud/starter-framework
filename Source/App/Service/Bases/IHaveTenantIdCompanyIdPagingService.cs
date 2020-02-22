using Project.Core.Enums.Framework;
using Project.Model.EntityBase;
using Project.RequestModel.Bases;
using Project.ViewModel;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public interface IHaveTenantIdCompanyIdPagingService <T, TVm> where T: HaveTenantIdCompanyIdEntityBase where TVm : HaveTenantIdCompanyIdViewModelBase<T>
    {
        ResponseModel<TVm> GetAllAsTenant(HaveTenantIdCompanyIdRequestModelBase<T> requestModel);

        ResponseModel<TVm> SearchAsTenant(PagingDataType status, HaveTenantIdCompanyIdRequestModelBase<T> requestModel);
    }
}