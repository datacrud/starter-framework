using System;
using System.Linq;
using Project.Core.Extensions;
using Project.Core.Helpers;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class CompanyViewModel : HaveTenantIdViewModelBase<Company>
    {
        public CompanyViewModel(Company model): base(model)
        {

            Name = model.Name;
            Slogan = model.Slogan;
            Description = model.Description;
            Phone = model.Phone;
            IsPhoneConfirmed = model.IsPhoneConfirmed;
            IsChangePhone = model.IsChangePhone;
            AwaitingConfirmPhone = model.AwaitingConfirmPhone;
            PhoneConfirmationCode = model.PhoneConfirmationCode;
            PhoneConfirmationCodeExpireTime = model.PhoneConfirmationCodeExpireTime;
            Email = model.Email;
            IsEmailConfirmed = model.IsEmailConfirmed;
            IsChangeEmail = model.IsChangeEmail;
            AwaitingConfirmEmail = model.AwaitingConfirmEmail;
            EmailConfirmationCode = model.EmailConfirmationCode;
            EmailConfirmationCodeExpireTime = model.EmailConfirmationCodeExpireTime;
            Address = model.Address;
            Web = model.Web;

            Logo = model.Logo;
            LogoFileType = model.LogoFileType;
            LogoName = model.LogoName;
            try
            {
                LogoPath = !string.IsNullOrWhiteSpace(model.LogoPath) && FileHelper.ReadRootPath != null ? FileHelper.ReadRootPath + model.LogoPath : "";
            }
            catch (Exception e)
            {
                LogoPath = null;
            }


            IsSendConfirmationEmail = !EmailConfirmationCode.IsNullEmptyOrWhiteSpace();

            TenantName = model.Tenant?.Name;
            TenantId = model.TenantId;

        }

        public string TenantName { get; set; }

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

        public string LogoViewPath =>
            !string.IsNullOrWhiteSpace(LogoPath) && !string.IsNullOrWhiteSpace(FileHelper.ReadRootPath)
                ? FileHelper.ReadRootPath + LogoPath
                : null;

        public CompanySettingsViewModel Settings { get; set; }


        public bool IsSendConfirmationEmail { get; set; }
        public string PoweredBy { get; set; }
    }
}