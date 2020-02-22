using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Employee : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        [Index("IX_Code", IsUnique = false, Order = 4, IsClustered = false), StringLength(100)]
        public string Code { get; set; }

        [Index("IX_Name", IsUnique = false, Order = 2, IsClustered = false), StringLength(256)]
        public string Name { get; set; }

        [Index("IX_Surname", IsUnique = false, Order = 2, IsClustered = false), StringLength(256)]
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";

        public EmployeeType Type { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BloodGroup { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nid { get; set; }
        public string Image { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string EmergencyContactRelation { get; set; }
        public string Note { get; set; }

        public double BasicSalary { get; set; }
        public double MonthlyGrossSalary { get; set; }
        public double YearlyGrossSalary { get; set; }


        //ignore properties
        public bool IsHostAction { get; set; }
    }
}