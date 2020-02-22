using Project.Core.Enums;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class WarehouseViewModel : HaveTenantIdCompanyIdViewModelBase<Warehouse>
    {
        public WarehouseViewModel(Warehouse model): base(model)
        {
            Code = model.Code;
            Name = model.Name;
            Address = model.Address;
            Type = model.Type;
            WarehouseType = model.Type.ToString();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public WarehouseType Type { get; set; }
        public string WarehouseType { get; set; }
    }
}