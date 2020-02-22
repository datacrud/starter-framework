using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Warehouse : HaveTenantIdCompanyIdEntityBase
    {
        //[Required]
        //[MaxLength(20)]
        //[Index("IX_Code", 2, IsUnique = true)]
        public string Code { get; set; }

        [Required]
        //[MaxLength(20)]
        //[Index("IX_Name", 3, IsUnique = true)]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        public WarehouseType Type { get; set; }

    }
}