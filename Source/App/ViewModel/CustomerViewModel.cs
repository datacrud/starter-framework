using Project.Core.Enums;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class CustomerViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<Customer>
    {
        public CustomerViewModel(Customer model) : base(model)
        {

            Identity = model.Identity;
            Code = model.Code;
            Name = model.Name;
            Type = model.Type;
            CustomerType = model.Type.ToString();
            Phone = model.Phone;
            Address = model.Address;
            Email = model.Email;
            Note = model.Note;

            OpeningDue = model.OpeningDue;
            TotalPayable = model.TotalPayable;
            TotalPaid = model.TotalPaid;
            TotalDue = model.TotalDue;
        }

        public int Identity { get; set; }       
        public string Code { get; set; }
        public string Name { get; set; }
        public CustomerType Type { get; set; }
        public string CustomerType { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }


        public double OpeningDue { get; set; }
        public double TotalPayable { get; set; }
        public double TotalPaid { get; set; }
        public double TotalDue { get; set; }
    }
}