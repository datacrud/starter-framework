using Project.Model.EntityBase;

namespace Project.ViewModel.Bases
{
    public abstract class HaveTenantIdCompanyIdViewModelBase<T> : ViewModelBase<T> where T : HaveTenantIdCompanyIdEntityBase
    {

        protected HaveTenantIdCompanyIdViewModelBase(T model)
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

            DeviceInfo = model.DeviceInfo;
            IpAddress = model.IpAddress;
        }

        public string TenantId { get; set; }
        public string CompanyId { get; set; }
    }
}