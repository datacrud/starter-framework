using System;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.DomainBase;
using Security.Models.Models;

namespace Project.Model.EntityBases
{
    public abstract class BusinessEntityBase : Entity<string>, IMayHaveOrder, IHaveIsActive, IMayHaveUserReference<User>
    {
        protected BusinessEntityBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool Active { get; set; } = true;


        [ForeignKey("CreatedBy")] public virtual User Creator { get; set; }


        [ForeignKey("ModifiedBy")] public virtual User Modifier { get; set; }

        [ForeignKey("DeletedBy")] public virtual User Deleter { get; set; }

        public int? Order { get; set; }
    }
}