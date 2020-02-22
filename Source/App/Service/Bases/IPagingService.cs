using Project.Core.DomainBase;
using Project.Core.Enums.Framework;
using Project.RequestModel.Bases;
using Project.ViewModel;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public interface IPagingService<T, TVm>
        where T : Entity<string>, IHaveIsActive, IMayHaveOrder //BusinessEntityBase 
        where TVm : ViewModelBase<T>
    {
        ResponseModel<TVm> GetAll(RequestModelBase<T> requestModel);

        ResponseModel<TVm> Search(PagingDataType status, RequestModelBase<T> requestModel);
    }


}