using System;
using System.Collections.Generic;
using System.Linq;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Model;

using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface ISupplierService : IHaveTenantIdCompanyIdBranchIdServiceBase<Supplier, SupplierViewModel>
    {

    }

    public class SupplierService : HaveTenantIdCompanyIdBranchIdServiceBase<Supplier, SupplierViewModel>, ISupplierService
    {
        private readonly ISupplierRepository _repository;

     
        public SupplierService(ISupplierRepository repository) : base(repository)
        {
            _repository = repository;
        }

    }
}