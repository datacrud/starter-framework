using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.ViewModel.Bases
{
    public abstract class HaveTenantIdViewModelBase<T> : ViewModelBase<T> where T : HaveTenantIdEntityBase
    {

        
        protected HaveTenantIdViewModelBase(T model)
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

            DeviceInfo = model.DeviceInfo;
            IpAddress = model.IpAddress;
        }

        public string TenantId { get; set; }
    }
}