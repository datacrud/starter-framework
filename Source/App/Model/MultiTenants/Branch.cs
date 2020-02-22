using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.Enums;
using Project.Model.EntityBase;

namespace Project.Model
{
    public class Branch : HaveTenantIdCompanyIdEntityBase
    {
        [Required]
        //[MaxLength(20)]
        //[Index("IX_Code", 2, IsUnique = true)]
        public string Code { get; set; }

        [Required]
        //[MaxLength(20)]
        //[Index("IX_Name", 3, IsUnique = true)]
        public string Name { get; set; }

        public string Address { get; set; }

        public BranchType Type { get; set; }

        public double OpeningCash { get; set; }


        public string LinkedWarehouseId { get; set; }
        [ForeignKey("LinkedWarehouseId")]
        public virtual Warehouse Warehouse { get; set; }


        //ignore properties
        public bool IsHostAction { get; set; }

    }
}