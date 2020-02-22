namespace Project.Model.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Initital_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branch",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        Type = c.Int(nullable: false),
                        OpeningCash = c.Double(nullable: false),
                        LinkedWarehouseId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Branch_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Branch_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .ForeignKey("dbo.Warehouse", t => t.LinkedWarehouseId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.LinkedWarehouseId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Slogan = c.String(),
                        Description = c.String(),
                        Phone = c.String(),
                        IsPhoneConfirmed = c.Boolean(nullable: false),
                        IsChangePhone = c.Boolean(nullable: false),
                        AwaitingConfirmPhone = c.String(),
                        PhoneConfirmationCode = c.String(),
                        PhoneConfirmationCodeExpireTime = c.DateTime(),
                        Email = c.String(),
                        IsEmailConfirmed = c.Boolean(nullable: false),
                        IsChangeEmail = c.Boolean(nullable: false),
                        AwaitingConfirmEmail = c.String(),
                        EmailConfirmationCode = c.String(),
                        EmailConfirmationCodeExpireTime = c.DateTime(),
                        Address = c.String(),
                        Web = c.String(),
                        Logo = c.Binary(),
                        LogoFileType = c.String(),
                        LogoName = c.String(),
                        LogoPath = c.String(),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Company_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Company_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TenantId = c.String(nullable: false, maxLength: 100),
                        CompanyId = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        TenantName = c.String(),
                        BranchId = c.String(),
                        EmployeeId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsChangePassword = c.Boolean(nullable: false),
                        PasswordChangeConfirmationCode = c.String(),
                        PasswordChangeConfirmationCodeExpireTime = c.DateTime(),
                        IsChangePhone = c.Boolean(nullable: false),
                        AwaitingConfirmPhone = c.String(),
                        PhoneConfirmationCode = c.String(),
                        PhoneConfirmationCodeExpireTime = c.DateTime(),
                        IsChangeEmail = c.Boolean(nullable: false),
                        AwaitingConfirmEmail = c.String(),
                        EmailConfirmationCode = c.String(),
                        EmailConfirmationCodeExpireTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        IsShouldChangedPasswordOnNextLogin = c.Boolean(nullable: false),
                        IsEnablePasswordExpiration = c.Boolean(nullable: false),
                        IsPasswordExpired = c.Boolean(nullable: false),
                        LastLoginTime = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_User_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .Index(t => new { t.UserName, t.TenantId }, unique: true, name: "UserNameIndex")
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenantId = c.String(),
                        CompanyId = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        TenantId = c.String(),
                        CompanyId = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        TenantId = c.String(),
                        CompanyId = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AppRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Tenant",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EditionId = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        ConnectionString = c.String(),
                        TenancyName = c.String(),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Url = c.String(),
                        LogoId = c.String(),
                        LogoFilePath = c.String(),
                        NoOfShowroom = c.Int(nullable: false),
                        IsInTrialPeriod = c.Boolean(nullable: false),
                        SubscriptionId = c.String(),
                        Package = c.Int(nullable: false),
                        SubscriptionEndTime = c.DateTime(),
                        BonusUserAccessCount = c.Int(nullable: false),
                        SpecialMonthlyDiscountPercentage = c.Int(nullable: false),
                        SpecialQuarterDiscountPercentage = c.Int(nullable: false),
                        SpecialHalfYearlyDiscountPercentage = c.Int(nullable: false),
                        SpecialAnnualDiscountPercentage = c.Int(nullable: false),
                        BusinessType = c.Int(),
                        IsDemo = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tenant_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Tenant_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.Edition", t => t.EditionId)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .Index(t => t.Id, unique: true)
                .Index(t => t.EditionId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Edition",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        DisplayName = c.String(),
                        Name = c.String(),
                        Descriminator = c.String(),
                        DiscountType = c.Int(nullable: false),
                        AnnualPriceDiscountAmount = c.Double(nullable: false),
                        AnnualPriceDiscountPercentage = c.Double(nullable: false),
                        HalfYearlyPriceDiscountAmount = c.Double(nullable: false),
                        HalfYearlyPriceDiscountPercentage = c.Double(nullable: false),
                        QuarterPriceDiscountAmount = c.Double(nullable: false),
                        QuarterPriceDiscountPercentage = c.Double(nullable: false),
                        MonthlyPriceDiscountAmount = c.Double(nullable: false),
                        MonthlyPriceDiscountPercentage = c.Double(nullable: false),
                        MinimumNoOfShowroom = c.Int(nullable: false),
                        MaximumNoOfShowroom = c.Int(nullable: false),
                        AnnualPrice = c.Double(nullable: false),
                        HalfYearlyPrice = c.Double(nullable: false),
                        QuarterPrice = c.Double(nullable: false),
                        MonthlyPrice = c.Double(nullable: false),
                        TrialDayCount = c.Int(nullable: false),
                        WaitingDayAfterExpire = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Edition_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Edition_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .Index(t => t.Id, unique: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Feature",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Value = c.String(),
                        ValueType = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        IsFeature = c.Boolean(nullable: false),
                        IsEditionFeature = c.Boolean(nullable: false),
                        IsTenantFeature = c.Boolean(nullable: false),
                        IsStatic = c.Boolean(nullable: false),
                        EditionId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Feature_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Feature_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.Edition", t => t.EditionId)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .Index(t => t.Id, unique: true)
                .Index(t => t.EditionId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Subscription",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TenantName = c.String(),
                        EditionId = c.String(maxLength: 128),
                        Package = c.Int(nullable: false),
                        PackageMonthlyPrice = c.Double(nullable: false),
                        PackageMonth = c.Int(nullable: false),
                        NoOfShowroom = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        PackageDiscountPercentage = c.Double(nullable: false),
                        PackageDiscountAmount = c.Double(nullable: false),
                        PackageCharge = c.Double(nullable: false),
                        ExpireOn = c.DateTime(),
                        RenewedOn = c.DateTime(),
                        Status = c.Int(nullable: false),
                        PaymentStatus = c.Int(nullable: false),
                        IsPaymentCompleted = c.Boolean(nullable: false),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Subscription_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Subscription_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.Edition", t => t.EditionId)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.EditionId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.SubscriptionPayment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SubscriptionId = c.String(maxLength: 128),
                        Date = c.DateTime(),
                        PaymentMethod = c.Int(nullable: false),
                        SubscriptionCharge = c.Double(nullable: false),
                        ServiceCharge = c.Double(nullable: false),
                        AmountToBePaid = c.Double(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        TransectionNumber = c.String(),
                        VerificationCode = c.String(),
                        PaymentReferance = c.String(),
                        PaymentStatus = c.Int(nullable: false),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SubscriptionPayment_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_SubscriptionPayment_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Subscription", t => t.SubscriptionId)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.SubscriptionId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Warehouse",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        Type = c.Int(nullable: false),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Warehouse_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Warehouse_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.CompanySetting",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BinNumber = c.String(),
                        TinNumber = c.String(),
                        CustomerInvoiceTermsAndConditions = c.String(),
                        SaleVatPercentage = c.Double(nullable: false),
                        DefineLowStockQuantity = c.String(),
                        IsUseDefaultSettings = c.Boolean(nullable: false),
                        EmailSenderDisplayName = c.String(),
                        NotificationSenderEmail = c.String(),
                        IsEnableEmailNotification = c.Boolean(nullable: false),
                        IsSendEmailToAdminForTransaction = c.Boolean(nullable: false),
                        IsSendEmailToCustomerForOrders = c.Boolean(nullable: false),
                        IsSendEmailToCustomerForDelivery = c.Boolean(nullable: false),
                        IsSendEmailToCustomerForTransaction = c.Boolean(nullable: false),
                        IsEnableSmsNotification = c.Boolean(nullable: false),
                        IsSendSmsToAdminForTransaction = c.Boolean(nullable: false),
                        IsSendSmsToCustomerForOrders = c.Boolean(nullable: false),
                        IsSendSmsToCustomerForDelivery = c.Boolean(nullable: false),
                        IsSendSmsToCustomerForTransaction = c.Boolean(nullable: false),
                        EnableStockLessFeatures = c.Boolean(nullable: false),
                        PoweredBy = c.String(),
                        HasYearlyHostingCharge = c.Boolean(nullable: false),
                        YearlyHostingChargeAmount = c.Double(nullable: false),
                        HostingValidTill = c.DateTime(),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompanySetting_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_CompanySetting_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Identity = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 256),
                        Type = c.Int(nullable: false),
                        Phone = c.String(maxLength: 50),
                        Address = c.String(),
                        Email = c.String(),
                        Note = c.String(),
                        OpeningDue = c.Double(nullable: false),
                        TotalPayable = c.Double(nullable: false),
                        TotalPaid = c.Double(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Customer_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Customer_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.Phone)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.EmailLog",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        EntityId = c.String(),
                        EntityInvoiceNo = c.String(),
                        EntityType = c.Int(nullable: false),
                        ActionType = c.Int(nullable: false),
                        EmailSenderUserId = c.String(),
                        EmailReceiverUserId = c.String(),
                        FromEmailAddress = c.String(),
                        ToEmailAddress = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        EmailType = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsUsedCarryForwardEmail = c.Boolean(nullable: false),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EmailLog_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_EmailLog_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(maxLength: 100),
                        Name = c.String(maxLength: 256),
                        Surname = c.String(maxLength: 256),
                        Type = c.Int(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        BloodGroup = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(),
                        Nid = c.String(),
                        Image = c.String(),
                        PresentAddress = c.String(),
                        PermanentAddress = c.String(),
                        EmergencyContactName = c.String(),
                        EmergencyContactPhone = c.String(),
                        EmergencyContactRelation = c.String(),
                        Note = c.String(),
                        BasicSalary = c.Double(nullable: false),
                        MonthlyGrossSalary = c.Double(nullable: false),
                        YearlyGrossSalary = c.Double(nullable: false),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Employee_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Employee_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.Surname)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.FiscalYear",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FiscalYear_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_FiscalYear_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.HostSetting",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Value = c.String(),
                        ValueType = c.Int(nullable: false),
                        IsStatic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoginAttempt",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(),
                        Username = c.String(),
                        Status = c.Int(nullable: false),
                        Error = c.String(),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LoginAttempt_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_LoginAttempt_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.MonthlyEmailBalance",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EditionId = c.String(),
                        MonthStartDate = c.DateTime(nullable: false),
                        MonthEndDate = c.DateTime(nullable: false),
                        Month = c.Int(nullable: false),
                        TotalSubscribeEmail = c.Int(nullable: false),
                        TotalSendEmail = c.Int(nullable: false),
                        TotalRemainingEmail = c.Int(nullable: false),
                        CarryForwardedEmailFromLastMonth = c.Int(nullable: false),
                        IsAllowSendEmailFromCarryForward = c.Boolean(nullable: false),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MonthlyEmailBalance_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MonthlyEmailBalance_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Partner",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        PartnerFrom = c.DateTime(nullable: false),
                        PrimaryContribution = c.Double(nullable: false),
                        Importance = c.String(),
                        Note = c.String(),
                        CurrentContribution = c.Double(nullable: false),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Partner_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Partner_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.AppPermissions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ResourceId = c.String(nullable: false, maxLength: 128),
                        TenantId = c.String(),
                        CompanyId = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Permission_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.AppResources", t => t.ResourceId, cascadeDelete: true)
                .ForeignKey("dbo.AppRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.RoleId)
                .Index(t => t.ResourceId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.AppResources",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        DisplayName = c.String(),
                        Route = c.String(nullable: false),
                        ParentRoute = c.String(),
                        IsPublic = c.Boolean(nullable: false),
                        Order = c.Int(),
                        ResourceOwner = c.Int(nullable: false),
                        TenantId = c.String(),
                        CompanyId = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Resource_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .Index(t => t.Id, unique: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.AppRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(),
                        AccessLevel = c.Int(nullable: false),
                        IsStatic = c.Boolean(nullable: false),
                        TenantId = c.String(),
                        CompanyId = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Role_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy)
                .Index(t => t.Name, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Rfq",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CompanyName = c.String(),
                        YourName = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                        TotalUsers = c.Int(nullable: false),
                        TotalBranch = c.Int(nullable: false),
                        TotalMonthlyEmailNotification = c.Int(nullable: false),
                        WhatINeed = c.String(),
                        HowYouKnowAboutUs = c.String(),
                        HowYouKnowAboutUsMessage = c.String(),
                        Comment = c.String(),
                        ResponseMessage = c.String(),
                        RfqStatus = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Rfq_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Rfq_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .Index(t => t.Id, unique: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 256),
                        Type = c.Int(),
                        Phone = c.String(maxLength: 50),
                        Email = c.String(),
                        Address = c.String(),
                        OpeningDue = c.Double(nullable: false),
                        TotalPaid = c.Double(nullable: false),
                        TotalDiscount = c.Double(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        Note = c.String(),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Supplier_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Supplier_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.Phone)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
            CreateTable(
                "dbo.AuditLog",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        EntityId = c.String(),
                        EntityType = c.Int(nullable: false),
                        EntityInvoiceNo = c.String(),
                        Data = c.String(),
                        ActionType = c.Int(nullable: false),
                        BranchId = c.String(maxLength: 128),
                        CompanyId = c.String(maxLength: 128),
                        TenantId = c.String(maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Order = c.Int(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeviceInfo = c.String(),
                        IpAddress = c.String(),
                        OriginalPk = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AuditLog_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_AuditLog_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.Id, unique: true)
                .Index(t => t.BranchId)
                .Index(t => t.CompanyId)
                .Index(t => t.TenantId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ModifiedBy)
                .Index(t => t.DeletedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuditLog", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.AuditLog", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditLog", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditLog", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditLog", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.AuditLog", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.Supplier", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.Supplier", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Supplier", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Supplier", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Supplier", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Supplier", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.Rfq", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rfq", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rfq", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppPermissions", "RoleId", "dbo.AppRoles");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AppRoles");
            DropForeignKey("dbo.AppRoles", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppRoles", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppRoles", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppPermissions", "ResourceId", "dbo.AppResources");
            DropForeignKey("dbo.AppResources", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppResources", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppResources", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppPermissions", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppPermissions", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppPermissions", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Partner", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.Partner", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Partner", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Partner", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Partner", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Partner", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.MonthlyEmailBalance", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.MonthlyEmailBalance", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MonthlyEmailBalance", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MonthlyEmailBalance", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.MonthlyEmailBalance", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.MonthlyEmailBalance", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.LoginAttempt", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.LoginAttempt", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.LoginAttempt", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.LoginAttempt", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.LoginAttempt", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.LoginAttempt", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.FiscalYear", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.FiscalYear", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.FiscalYear", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.FiscalYear", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.FiscalYear", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.FiscalYear", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.Employee", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.Employee", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Employee", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Employee", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Employee", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Employee", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.EmailLog", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.EmailLog", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmailLog", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmailLog", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmailLog", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.EmailLog", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.Customer", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.Customer", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customer", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customer", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customer", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Customer", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.CompanySetting", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.CompanySetting", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanySetting", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanySetting", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanySetting", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Branch", "LinkedWarehouseId", "dbo.Warehouse");
            DropForeignKey("dbo.Warehouse", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.Warehouse", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Warehouse", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Warehouse", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Warehouse", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Branch", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.Branch", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Branch", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Branch", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Branch", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Company", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.Tenant", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tenant", "EditionId", "dbo.Edition");
            DropForeignKey("dbo.Subscription", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.SubscriptionPayment", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.SubscriptionPayment", "SubscriptionId", "dbo.Subscription");
            DropForeignKey("dbo.SubscriptionPayment", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubscriptionPayment", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubscriptionPayment", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubscriptionPayment", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Subscription", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subscription", "EditionId", "dbo.Edition");
            DropForeignKey("dbo.Subscription", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subscription", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subscription", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Edition", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feature", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feature", "EditionId", "dbo.Edition");
            DropForeignKey("dbo.Feature", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feature", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Edition", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Edition", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tenant", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tenant", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Company", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Company", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Company", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "ModifiedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "DeletedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.AuditLog", new[] { "DeletedBy" });
            DropIndex("dbo.AuditLog", new[] { "ModifiedBy" });
            DropIndex("dbo.AuditLog", new[] { "CreatedBy" });
            DropIndex("dbo.AuditLog", new[] { "TenantId" });
            DropIndex("dbo.AuditLog", new[] { "CompanyId" });
            DropIndex("dbo.AuditLog", new[] { "BranchId" });
            DropIndex("dbo.AuditLog", new[] { "Id" });
            DropIndex("dbo.Supplier", new[] { "DeletedBy" });
            DropIndex("dbo.Supplier", new[] { "ModifiedBy" });
            DropIndex("dbo.Supplier", new[] { "CreatedBy" });
            DropIndex("dbo.Supplier", new[] { "TenantId" });
            DropIndex("dbo.Supplier", new[] { "CompanyId" });
            DropIndex("dbo.Supplier", new[] { "BranchId" });
            DropIndex("dbo.Supplier", new[] { "Phone" });
            DropIndex("dbo.Supplier", new[] { "Name" });
            DropIndex("dbo.Supplier", new[] { "Code" });
            DropIndex("dbo.Supplier", new[] { "Id" });
            DropIndex("dbo.Rfq", new[] { "DeletedBy" });
            DropIndex("dbo.Rfq", new[] { "ModifiedBy" });
            DropIndex("dbo.Rfq", new[] { "CreatedBy" });
            DropIndex("dbo.Rfq", new[] { "Id" });
            DropIndex("dbo.AppRoles", "RoleNameIndex");
            DropIndex("dbo.AppRoles", new[] { "DeletedBy" });
            DropIndex("dbo.AppRoles", new[] { "ModifiedBy" });
            DropIndex("dbo.AppRoles", new[] { "CreatedBy" });
            DropIndex("dbo.AppResources", new[] { "DeletedBy" });
            DropIndex("dbo.AppResources", new[] { "ModifiedBy" });
            DropIndex("dbo.AppResources", new[] { "CreatedBy" });
            DropIndex("dbo.AppResources", new[] { "Id" });
            DropIndex("dbo.AppPermissions", new[] { "DeletedBy" });
            DropIndex("dbo.AppPermissions", new[] { "ModifiedBy" });
            DropIndex("dbo.AppPermissions", new[] { "CreatedBy" });
            DropIndex("dbo.AppPermissions", new[] { "ResourceId" });
            DropIndex("dbo.AppPermissions", new[] { "RoleId" });
            DropIndex("dbo.AppPermissions", new[] { "Id" });
            DropIndex("dbo.Partner", new[] { "DeletedBy" });
            DropIndex("dbo.Partner", new[] { "ModifiedBy" });
            DropIndex("dbo.Partner", new[] { "CreatedBy" });
            DropIndex("dbo.Partner", new[] { "TenantId" });
            DropIndex("dbo.Partner", new[] { "CompanyId" });
            DropIndex("dbo.Partner", new[] { "BranchId" });
            DropIndex("dbo.Partner", new[] { "Id" });
            DropIndex("dbo.MonthlyEmailBalance", new[] { "DeletedBy" });
            DropIndex("dbo.MonthlyEmailBalance", new[] { "ModifiedBy" });
            DropIndex("dbo.MonthlyEmailBalance", new[] { "CreatedBy" });
            DropIndex("dbo.MonthlyEmailBalance", new[] { "TenantId" });
            DropIndex("dbo.MonthlyEmailBalance", new[] { "CompanyId" });
            DropIndex("dbo.MonthlyEmailBalance", new[] { "BranchId" });
            DropIndex("dbo.MonthlyEmailBalance", new[] { "Id" });
            DropIndex("dbo.LoginAttempt", new[] { "DeletedBy" });
            DropIndex("dbo.LoginAttempt", new[] { "ModifiedBy" });
            DropIndex("dbo.LoginAttempt", new[] { "CreatedBy" });
            DropIndex("dbo.LoginAttempt", new[] { "TenantId" });
            DropIndex("dbo.LoginAttempt", new[] { "CompanyId" });
            DropIndex("dbo.LoginAttempt", new[] { "BranchId" });
            DropIndex("dbo.LoginAttempt", new[] { "Id" });
            DropIndex("dbo.FiscalYear", new[] { "DeletedBy" });
            DropIndex("dbo.FiscalYear", new[] { "ModifiedBy" });
            DropIndex("dbo.FiscalYear", new[] { "CreatedBy" });
            DropIndex("dbo.FiscalYear", new[] { "TenantId" });
            DropIndex("dbo.FiscalYear", new[] { "CompanyId" });
            DropIndex("dbo.FiscalYear", new[] { "BranchId" });
            DropIndex("dbo.FiscalYear", new[] { "Id" });
            DropIndex("dbo.Employee", new[] { "DeletedBy" });
            DropIndex("dbo.Employee", new[] { "ModifiedBy" });
            DropIndex("dbo.Employee", new[] { "CreatedBy" });
            DropIndex("dbo.Employee", new[] { "TenantId" });
            DropIndex("dbo.Employee", new[] { "CompanyId" });
            DropIndex("dbo.Employee", new[] { "BranchId" });
            DropIndex("dbo.Employee", new[] { "Surname" });
            DropIndex("dbo.Employee", new[] { "Name" });
            DropIndex("dbo.Employee", new[] { "Code" });
            DropIndex("dbo.Employee", new[] { "Id" });
            DropIndex("dbo.EmailLog", new[] { "DeletedBy" });
            DropIndex("dbo.EmailLog", new[] { "ModifiedBy" });
            DropIndex("dbo.EmailLog", new[] { "CreatedBy" });
            DropIndex("dbo.EmailLog", new[] { "TenantId" });
            DropIndex("dbo.EmailLog", new[] { "CompanyId" });
            DropIndex("dbo.EmailLog", new[] { "BranchId" });
            DropIndex("dbo.EmailLog", new[] { "Id" });
            DropIndex("dbo.Customer", new[] { "DeletedBy" });
            DropIndex("dbo.Customer", new[] { "ModifiedBy" });
            DropIndex("dbo.Customer", new[] { "CreatedBy" });
            DropIndex("dbo.Customer", new[] { "TenantId" });
            DropIndex("dbo.Customer", new[] { "CompanyId" });
            DropIndex("dbo.Customer", new[] { "BranchId" });
            DropIndex("dbo.Customer", new[] { "Phone" });
            DropIndex("dbo.Customer", new[] { "Name" });
            DropIndex("dbo.Customer", new[] { "Code" });
            DropIndex("dbo.Customer", new[] { "Id" });
            DropIndex("dbo.CompanySetting", new[] { "DeletedBy" });
            DropIndex("dbo.CompanySetting", new[] { "ModifiedBy" });
            DropIndex("dbo.CompanySetting", new[] { "CreatedBy" });
            DropIndex("dbo.CompanySetting", new[] { "TenantId" });
            DropIndex("dbo.CompanySetting", new[] { "CompanyId" });
            DropIndex("dbo.CompanySetting", new[] { "Id" });
            DropIndex("dbo.Warehouse", new[] { "DeletedBy" });
            DropIndex("dbo.Warehouse", new[] { "ModifiedBy" });
            DropIndex("dbo.Warehouse", new[] { "CreatedBy" });
            DropIndex("dbo.Warehouse", new[] { "TenantId" });
            DropIndex("dbo.Warehouse", new[] { "CompanyId" });
            DropIndex("dbo.Warehouse", new[] { "Id" });
            DropIndex("dbo.SubscriptionPayment", new[] { "DeletedBy" });
            DropIndex("dbo.SubscriptionPayment", new[] { "ModifiedBy" });
            DropIndex("dbo.SubscriptionPayment", new[] { "CreatedBy" });
            DropIndex("dbo.SubscriptionPayment", new[] { "TenantId" });
            DropIndex("dbo.SubscriptionPayment", new[] { "CompanyId" });
            DropIndex("dbo.SubscriptionPayment", new[] { "SubscriptionId" });
            DropIndex("dbo.SubscriptionPayment", new[] { "Id" });
            DropIndex("dbo.Subscription", new[] { "DeletedBy" });
            DropIndex("dbo.Subscription", new[] { "ModifiedBy" });
            DropIndex("dbo.Subscription", new[] { "CreatedBy" });
            DropIndex("dbo.Subscription", new[] { "TenantId" });
            DropIndex("dbo.Subscription", new[] { "CompanyId" });
            DropIndex("dbo.Subscription", new[] { "EditionId" });
            DropIndex("dbo.Subscription", new[] { "Id" });
            DropIndex("dbo.Feature", new[] { "DeletedBy" });
            DropIndex("dbo.Feature", new[] { "ModifiedBy" });
            DropIndex("dbo.Feature", new[] { "CreatedBy" });
            DropIndex("dbo.Feature", new[] { "EditionId" });
            DropIndex("dbo.Feature", new[] { "Id" });
            DropIndex("dbo.Edition", new[] { "DeletedBy" });
            DropIndex("dbo.Edition", new[] { "ModifiedBy" });
            DropIndex("dbo.Edition", new[] { "CreatedBy" });
            DropIndex("dbo.Edition", new[] { "Id" });
            DropIndex("dbo.Tenant", new[] { "DeletedBy" });
            DropIndex("dbo.Tenant", new[] { "ModifiedBy" });
            DropIndex("dbo.Tenant", new[] { "CreatedBy" });
            DropIndex("dbo.Tenant", new[] { "EditionId" });
            DropIndex("dbo.Tenant", new[] { "Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "DeletedBy" });
            DropIndex("dbo.AspNetUserRoles", new[] { "ModifiedBy" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "DeletedBy" });
            DropIndex("dbo.AspNetUserLogins", new[] { "ModifiedBy" });
            DropIndex("dbo.AspNetUserLogins", new[] { "CreatedBy" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "DeletedBy" });
            DropIndex("dbo.AspNetUserClaims", new[] { "ModifiedBy" });
            DropIndex("dbo.AspNetUserClaims", new[] { "CreatedBy" });
            DropIndex("dbo.AspNetUsers", new[] { "DeletedBy" });
            DropIndex("dbo.AspNetUsers", new[] { "ModifiedBy" });
            DropIndex("dbo.AspNetUsers", new[] { "CreatedBy" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Company", new[] { "DeletedBy" });
            DropIndex("dbo.Company", new[] { "ModifiedBy" });
            DropIndex("dbo.Company", new[] { "CreatedBy" });
            DropIndex("dbo.Company", new[] { "TenantId" });
            DropIndex("dbo.Company", new[] { "Id" });
            DropIndex("dbo.Branch", new[] { "DeletedBy" });
            DropIndex("dbo.Branch", new[] { "ModifiedBy" });
            DropIndex("dbo.Branch", new[] { "CreatedBy" });
            DropIndex("dbo.Branch", new[] { "TenantId" });
            DropIndex("dbo.Branch", new[] { "CompanyId" });
            DropIndex("dbo.Branch", new[] { "LinkedWarehouseId" });
            DropIndex("dbo.Branch", new[] { "Id" });
            DropTable("dbo.AuditLog",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AuditLog_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_AuditLog_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Supplier",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Supplier_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Supplier_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Rfq",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Rfq_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Rfq_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AppRoles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Role_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AppResources",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Resource_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AppPermissions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Permission_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Partner",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Partner_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Partner_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MonthlyEmailBalance",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MonthlyEmailBalance_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_MonthlyEmailBalance_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LoginAttempt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LoginAttempt_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_LoginAttempt_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.HostSetting");
            DropTable("dbo.FiscalYear",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FiscalYear_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_FiscalYear_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Employee",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Employee_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Employee_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EmailLog",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EmailLog_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_EmailLog_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Customer",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Customer_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Customer_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CompanySetting",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompanySetting_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_CompanySetting_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Warehouse",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Warehouse_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Warehouse_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SubscriptionPayment",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SubscriptionPayment_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_SubscriptionPayment_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Subscription",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Subscription_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Subscription_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Feature",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Feature_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Feature_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Edition",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Edition_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Edition_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Tenant",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tenant_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Tenant_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_User_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Company",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Company_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Company_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Branch",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Branch_IsActive", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Branch_IsDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
