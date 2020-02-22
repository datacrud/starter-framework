using Project.Core.Enums.Framework;
using Project.Model.EntityBase;
using Project.Model.EntityBases;
using Project.RequestModel.Bases;
using Project.ViewModel;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public interface IHaveTenantIdCompanyIdBranchIdPagingService <T, TVm> where T: HaveTenantIdCompanyIdBranchIdEntityBase where TVm : HaveTenantIdCompanyIdBranchIdViewModelBase<T>
    {
        /// <summary>
        /// Get paging data filter by tenant
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        ResponseModel<TVm> GetAllAsTenant(HaveTenantIdCompanyIdBranchIdRequestModelBase<T> requestModel);

        /// <summary>
        /// Search paging data filter by tenant
        /// </summary>
        /// <param name="status"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        ResponseModel<TVm> SearchAsTenant(PagingDataType status, HaveTenantIdCompanyIdBranchIdRequestModelBase<T> requestModel);
    }
}