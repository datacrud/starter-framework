using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace Project.Model
{
    public class Customer : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        public int Identity { get; set; }

        [Index("IX_Code", IsUnique = false, Order = 4, IsClustered = false), StringLength(50)]
        public string Code { get; set; }

        [Index("IX_Name", IsUnique = false, Order = 3, IsClustered = false), StringLength(256)]
        [Required]
        public string Name { get; set; }

        [Required]
        [DefaultValue(CustomerType.Consumer)]
        public CustomerType Type { get; set; }


        [Index("IX_Phone", IsUnique = false, Order = 2, IsClustered = false), StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string Address { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        public string Note { get; set; }        


        public double OpeningDue { get; set; }

        public double TotalPayable { get; set; }
        public double TotalPaid { get; set; }
        public double TotalDue { get; set; }

    }

}