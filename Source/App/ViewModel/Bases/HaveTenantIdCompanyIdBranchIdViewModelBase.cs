using System;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.ViewModel.Bases
{
    public abstract class HaveTenantIdCompanyIdBranchIdViewModelBase<T> : ViewModelBase<T> where T : HaveTenantIdCompanyIdBranchIdEntityBase
    {

        protected HaveTenantIdCompanyIdBranchIdViewModelBase(T model): base(model)
        {
            Id = model.Id;
            Created = model.Created;
            CreatedBy = model.CreatedBy;
            Modified = model.Modified;
            ModifiedBy = model.ModifiedBy;
            Active = model.Active;

            IsDeleted = model.IsDeleted;
            Deleted = model.Deleted;
            DeletedBy = model.DeletedBy;

            TenantId = model.TenantId;
            CompanyId = model.CompanyId;
            BranchId = model.BranchId;

            if (model.Branch != null) Branch = new BranchViewModel(model.Branch);

            DeviceInfo = model.DeviceInfo;
            IpAddress = model.IpAddress;

            Order = model.Order;
        }

        public string TenantId { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }

        public BranchViewModel Branch { get; set; }

        public int? Order { get; set; }
    }
}
