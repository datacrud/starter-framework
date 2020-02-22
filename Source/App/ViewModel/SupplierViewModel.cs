using Project.Core.Enums;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class SupplierViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<Supplier>
    {
        public SupplierViewModel(Supplier model): base(model)
        {
            
            Code = model.Code;
            Name = model.Name;            
            Type = model.Type;
            SupplierType = model.Type.ToString();
            Phone = model.Phone;
            Email = model.Email;
            Address = model.Address;
            Note = model.Note;

          
            OpeningDue = model.OpeningDue;

            TotalPaid = model.TotalPaid;
            TotalDiscount = model.TotalDiscount;
            TotalDue = model.TotalDue;


        }

        public string Code { get; set; }
        public string Name { get; set; }
        public SupplierType? Type { get; set; }
        public string SupplierType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }

        
        public double OpeningDue { get; set; }

        public double TotalPaid { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalDue { get; set; }

       
    }
}