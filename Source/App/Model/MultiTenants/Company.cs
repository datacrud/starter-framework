using System;
using System.IO;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Company : HaveTenantIdEntityBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }

        public string Phone { get; set; }
        public bool IsPhoneConfirmed { get; set; }
        public bool IsChangePhone { get; set; }
        public string AwaitingConfirmPhone { get; set; }
        public string PhoneConfirmationCode { get; set; }
        public DateTime? PhoneConfirmationCodeExpireTime { get; set; }

        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsChangeEmail { get; set; }
        public string AwaitingConfirmEmail { get; set; }
        public string EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpireTime { get; set; }

        public string Address { get; set; }
        public string Web { get; set; }


        public byte[] Logo { get; set; }
        public string LogoFileType { get; set; }
        public string LogoName { get; set; }
        public string LogoPath { get; set; }


        //ignore properties
        public bool IsHostAction { get; set; }
    }
}