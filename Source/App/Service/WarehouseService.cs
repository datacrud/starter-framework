using System.Collections.Generic;
using System.Linq;
using Project.Core.Enums.Framework;
using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IWarehouseService : IHaveTenantIdCompanyIdServiceBase<Warehouse, WarehouseViewModel>
    {
        bool IsWarehouseNameExist(string name, string id, ActionType actionType);
        bool IsWarehouseCodeExist(string code, string id, ActionType actionType);
        bool IsReachedMaximumWarehouseCount(string tenantId, int featureMaximumWarehouse);
    }

    public class WarehouseService : HaveTenantIdCompanyIdServiceBase<Warehouse, WarehouseViewModel>, IWarehouseService
    {
        private readonly IWarehouseRepository _repository;
       

        public WarehouseService(IWarehouseRepository repository) : base(repository)
        {
            _repository = repository;

        }

        public bool IsWarehouseNameExist(string name, string id, ActionType actionType)
        {
            var category = _repository.AsNoTracking().FirstOrDefault(x => x.Name == name);
            if (category != null && category.Id != id) return true;
            
            return false;
        }

        public bool IsWarehouseCodeExist(string code, string id, ActionType actionType)
        {
            var category = _repository.AsNoTracking().FirstOrDefault(x => x.Code == code);
            if (category != null && category.Id != id) return true;
            
            return false;
        }

        public bool IsReachedMaximumWarehouseCount(string tenantId, int featureMaximumWarehouse)
        {
            var count = _repository.AsNoTracking().Count(x => x.TenantId == tenantId);
            return count >= featureMaximumWarehouse;
        }
    }
}
