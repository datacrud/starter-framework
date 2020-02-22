using System;
using Project.Core.DomainBase;

namespace Project.ViewModel.Bases
{
    public abstract class ViewModelBase<T> where T : Entity<string>, IHaveIsActive, IMayHaveOrder //BusinessEntityBase
    {

        protected ViewModelBase() { }

        protected ViewModelBase(T model)
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


            DeviceInfo = model.DeviceInfo;
            IpAddress = model.IpAddress;
        }

        public string Id { get; set; }

        public bool Active { get; set; }

        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime? Deleted { get; set; }
        public string DeletedBy { get; set; }


        public string DeviceInfo { get; set; }
        public string IpAddress { get; set; }
    }
}