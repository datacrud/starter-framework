using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.DomainBase
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        [Key]
        [Index("IX_Id", 1, IsUnique = true)]
        public TKey Id { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }

        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        

        public bool IsDeleted { get; set; }
        public DateTime? Deleted { get; set; }
        public string DeletedBy { get; set; }

        public string DeviceInfo { get; set; }
        public string IpAddress { get; set; }



        public string OriginalPk { get; set; }
    }
}