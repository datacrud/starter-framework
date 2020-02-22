using System;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class EmployeeViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<Employee>
    {
        public EmployeeViewModel(Employee model) : base(model)
        {
            Name = model.Name;
            Surname = model.Surname;
            Code = model.Code;
            Type = model.Type;
            TypeName = model.Type.GetDescription();
            Phone = model.Phone;
            Email = model.Email;
            BloodGroup = model.BloodGroup;
            Gender = model.Gender;
            DateOfBirth = model.DateOfBirth;
            Nid = model.Nid;
            Image = model.Image;
            PresentAddress = model.PresentAddress;
            PermanentAddress = model.PermanentAddress;
            EmergencyContactName = model.EmergencyContactName;
            EmergencyContactPhone = model.EmergencyContactPhone;
            EmergencyContactRelation = model.EmergencyContactRelation;
            Note = model.Note;

            BasicSalary = model.BasicSalary;
            MonthlyGrossSalary = model.MonthlyGrossSalary;
            YearlyGrossSalary = model.YearlyGrossSalary;
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public EmployeeType Type { get; set; }
        public string TypeName { get; set; }
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
    }
}