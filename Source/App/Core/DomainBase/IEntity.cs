using System;

namespace Project.Core.DomainBase
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }

        DateTime Created { get; set; }
        string CreatedBy { get; set; }

        DateTime? Modified { get; set; }
        string ModifiedBy { get; set; }


        bool IsDeleted { get; set; }
        DateTime? Deleted { get; set; }
        string DeletedBy { get; set; }

        string DeviceInfo { get; set; }
        string IpAddress { get; set; }
    }
}