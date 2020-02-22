using System;
using System.Linq;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Model;
using Project.Repository;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface ICustomerService : IHaveTenantIdCompanyIdBranchIdServiceBase<Customer, CustomerViewModel>
    {
        bool IsCustomerPhoneExist(string phone, string id, ActionType actionType);
    }

    public class CustomerService : HaveTenantIdCompanyIdBranchIdServiceBase<Customer, CustomerViewModel>, ICustomerService
    {
        private readonly ICustomerRepository _repository;
        

        public CustomerService(ICustomerRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public override ResponseModel<CustomerViewModel> GetAllAsTenant(HaveTenantIdCompanyIdBranchIdRequestModelBase<Customer> requestModel)
        {
            var queryable = GetPagingQuery(Repository.GetAllAsTenant(), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<CustomerViewModel>(entities.OrderBy(x=> x.Name).ToList(), Repository.GetAllAsTenant().Count());
            //Repository.Dispose();

            return response;
        }


        public bool IsCustomerPhoneExist(string phone, string id, ActionType actionType)
        {
            var customer = _repository.AsNoTracking().FirstOrDefault(x => x.Phone == phone);
            if (customer != null && customer.Id != id) return true;

            return false;
        }
    }
}