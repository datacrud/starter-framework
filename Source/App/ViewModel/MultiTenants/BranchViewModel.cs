using System;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class BranchViewModel : HaveTenantIdCompanyIdViewModelBase<Branch>, IDisposable
    {
        private readonly BusinessDbContext _db = new BusinessDbContext();

        public BranchViewModel(Branch model): base(model)
        {
            Code = model.Code;
            Name = model.Name;
            Address = model.Address;
            Type = model.Type;
            BranchType = model.Type.GetDescription();
            OpeningCash = model.OpeningCash;
            LinkedWarehouseId = model.LinkedWarehouseId;

            LinkedWarehouseName = SetWarehouseName(model.LinkedWarehouseId);
        }

        private string SetWarehouseName(string id)
        {
            var warehouse = _db.Warehouses.Find(id);
            if (warehouse == null) return "";
            return warehouse.Name;
        }



        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public BranchType Type { get; set; }
        public string BranchType { get; set; }
        public double OpeningCash { get; set; }


        public string LinkedWarehouseId { get; set; }

        public string LinkedWarehouseName { get; set; }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}