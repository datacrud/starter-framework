using System.Collections.Generic;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Core.StaticResource.Models;

namespace Project.Core.StaticResource
{
    public class StaticResource
    {
        public class Public
        {
            public static StaticResourceDto Login =
                new StaticResourceDto(ResourceOwner.Both, "Login", "Login", "root.login", "/login", true);

            public static StaticResourceDto Register =
                new StaticResourceDto(ResourceOwner.Both, "Register", "Register", "root.register", "/register", true);

            public static StaticResourceDto Rfq = new StaticResourceDto(ResourceOwner.Both, "Rfq", "Rfq", "root.rfq", "/rfq", true);

            public static StaticResourceDto ForgetPassword = new StaticResourceDto(ResourceOwner.Both, "Forget Password",
                "Forget Password", "root.forgot-password", "/forgot-password", true);

            public static StaticResourceDto ResetPassword = new StaticResourceDto(ResourceOwner.Both, "Reset Password",
                "Reset Password", "root.reset-password", "/reset-password", true);

            public static StaticResourceDto EmailVerification = new StaticResourceDto(ResourceOwner.Both, "Email Verification",
                "Email Verification", "root.email-verification", "/email-verification", true);

            public static StaticResourceDto AccessDenied = new StaticResourceDto(ResourceOwner.Both, "Access Denied", "Access Denied",
                "root.access-denied", "/access-denied", true);

            public static StaticResourceDto About = new StaticResourceDto(ResourceOwner.Both, "About", "About", "root.about", "/about", true);

            public static StaticResourceDto Contact = new StaticResourceDto(ResourceOwner.Both, "Contact", "Contact", "root.contact", "/contact", true);

            public static List<StaticResourceDto> GetAllPublicResources()
            {
                return new List<StaticResourceDto>
                {
                    Login,
                    Register,
                    ForgetPassword,
                    ResetPassword,
                    EmailVerification,
                    AccessDenied,
                    About,
                    Contact
                };
            }
        }



        public class Private
        {
            public class DataPermission
            {
                public static StaticResourceDto DataPermissionParent =
                    new StaticResourceDto(ResourceOwner.Both, "DataPermission", "Data Permission",
                        "root.data-permission", null, false, 0);

                public static StaticResourceDto CompanyData = new StaticResourceDto(ResourceOwner.Both, "CompanyData",
                    "Company Data", "root.data-permission.company-data");

                public static StaticResourceDto UserBranchData = new StaticResourceDto(ResourceOwner.Both, "UserBranchData",
                    "User Branch Data", "root.data-permission.user-branch-data");

                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        DataPermissionParent,
                        CompanyData.AddParent(DataPermissionParent),
                        UserBranchData.AddParent(DataPermissionParent)
                    };


                    return list;
                }
            }


            public class MultiTenant
            {
                public static StaticResourceDto SystemAdministrationParent =
                    new StaticResourceDto(ResourceOwner.Both, "SystemAdministration", "System Administration",
                        "root.sysadmin", null, false, 1);


                public static StaticResourceDto Feature =
                    new StaticResourceDto(ResourceOwner.Host, "Feature", "Feature", "root.feature", "/feature");

                public static StaticResourceDto Edition = new StaticResourceDto(ResourceOwner.Host, "Edition", "Edition", "root.edition", "/edition");

                public static StaticResourceDto Tenant = new StaticResourceDto(ResourceOwner.Host, "Tenant", "Tenant", "root.tenant", "/tenant");

                public static StaticResourceDto Subscription = new StaticResourceDto(ResourceOwner.Host, "Subscription",
                    "Subscription", "root.subscription", "subscription");

                public static StaticResourceDto Payment = new StaticResourceDto(ResourceOwner.Host, "Payment", "Payment", "root.payment");

                public static StaticResourceDto ManageSubscription = new StaticResourceDto(ResourceOwner.Both, "Manage Subscription", "Manage Subscription", "root.manage-subscription", "/manage-subscription");

                public static StaticResourceDto SubscriptionPayment = new StaticResourceDto(ResourceOwner.Both, "Subscription Payment", "Subscription Payment", "root.subscription-payment", "/subscription-payment");

                public static StaticResourceDto TenantUser = new StaticResourceDto(ResourceOwner.Host, "Tenant User", "Tenant User", "root.tenant-user");

                public static StaticResourceDto TenantCompany = new StaticResourceDto(ResourceOwner.Host, "Tenant Company", "Tenant Company", "root.tenant-company");

                public static StaticResourceDto RfqManager = new StaticResourceDto(ResourceOwner.Host, "RFQ Manager", "RFQ Manager", "root.rfq-manager");

                public static StaticResourceDto Role = new StaticResourceDto(ResourceOwner.Both, "Role", "Role", "root.role");

                public static StaticResourceDto Resource = new StaticResourceDto(ResourceOwner.Host, "Resource", "Resource", "root.resource");

                public static StaticResourceDto Permission = new StaticResourceDto(ResourceOwner.Both, "Permission", "Permission", "root.permission");


                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        SystemAdministrationParent,
                        Feature.AddParent(SystemAdministrationParent),
                        Edition.AddParent(SystemAdministrationParent),
                        Tenant.AddParent(SystemAdministrationParent),
                        Subscription.AddParent(SystemAdministrationParent),
                        Payment.AddParent(SystemAdministrationParent),
                        ManageSubscription.AddParent(SystemAdministrationParent),
                        SubscriptionPayment.AddParent(SystemAdministrationParent),
                        TenantUser.AddParent(SystemAdministrationParent),
                        TenantCompany.AddParent(SystemAdministrationParent),
                        RfqManager.AddParent(SystemAdministrationParent),

                        Role.AddParent(SystemAdministrationParent),
                        Resource.AddParent(SystemAdministrationParent),
                        Permission.AddParent(SystemAdministrationParent),
                    };

                    list.AddRange(Feature.AddCrudChildren());
                    list.AddRange(Edition.AddCrudChildren());
                    list.AddRange(Tenant.AddCrudChildren());
                    list.AddRange(Subscription.AddCrudChildren());
                    list.AddRange(Payment.AddCrudChildren());

                    list.Add(ManageSubscription.AddViewChildren());
                    list.Add(ManageSubscription.AddCreateChildren());

                    list.Add(SubscriptionPayment.AddViewChildren());
                    list.Add(SubscriptionPayment.AddCreateChildren());

                    list.AddRange(TenantUser.AddCrudChildren());
                    list.AddRange(TenantCompany.AddCrudChildren());
                    list.AddRange(RfqManager.AddCrudChildren());

                    list.AddRange(Resource.AddCrudChildren());
                    list.Add(Permission.AddViewChildren());
                    list.Add(Permission.AddEditChildren());


                    list.AddRange(Role.AddCrudChildren());
                    list.Add(Role.AddChildren("Permission", $"{Role.State}.permission"));


                    return list;
                }
            }

            public class Common
            {
                public static StaticResourceDto CommonParent =
                    new StaticResourceDto(ResourceOwner.Both, "Common", "Common", "root.common", null, false, 2);

                public static StaticResourceDto Profile = new StaticResourceDto(ResourceOwner.Both, "Profile", "Profile", "root.profile", "/profile");

                public static StaticResourceDto LoginAttempt = new StaticResourceDto(ResourceOwner.Both, "Login Attempt", "Login Attempt", "root.login-attempt", "/login-attempt");

                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        CommonParent,

                        //Child
                        Profile.AddParent(CommonParent),
                        LoginAttempt.AddParent(CommonParent),

                        //Crud Child
                        Profile.AddViewChildren(),
                        Profile.AddEditChildren(),
                        LoginAttempt.AddViewChildren()
                    };



                    return list;
                }
            }

            public class Landing
            {
                public static StaticResourceDto LandingParent =
                    new StaticResourceDto(ResourceOwner.Both, "Landing", "Landing", "root.landing", null, false, 3);

                public static StaticResourceDto Home = new StaticResourceDto(ResourceOwner.Both, "Home", "Home", "root.home", "/home");

                public static StaticResourceDto Dashboard = new StaticResourceDto(ResourceOwner.Both, "Dashboard", "Dashboard", "root.dashboard", "/dashboard");

                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        LandingParent,

                        //Child
                        Home.AddParent(LandingParent),
                        Dashboard.AddParent(LandingParent),

                        //Custom Child
                        Dashboard.AddChildren("View Note", "root.dashboard.view-note"),
                        Dashboard.AddChildren("Today Summary", "root.dashboard.today-summary")
                    };



                    return list;
                }
            }


            public class Administration
            {
                public static StaticResourceDto AdministrationParent =
                    new StaticResourceDto(ResourceOwner.Tenant, "Administration", "Administration", "root.administration", null, false, 4);

                public static StaticResourceDto Company = new StaticResourceDto(ResourceOwner.Tenant, "Company", "Setup My Company", "root.company", "/company");
                public static StaticResourceDto CompanyProfile = new StaticResourceDto(ResourceOwner.Tenant, "CompanyProfile", "Profile", "root.company.profile");
                public static StaticResourceDto CompanySettings = new StaticResourceDto(ResourceOwner.Tenant, "CompanySettings", "Settings", "root.company.settings");
                public static StaticResourceDto CompanyLogo = new StaticResourceDto(ResourceOwner.Tenant, "CompanyLogo", "Logo", "root.company.logo");
                public static StaticResourceDto CompanyFiscalYear = new StaticResourceDto(ResourceOwner.Tenant, "CompanyFiscalYear", "Fiscal Year", "root.company.fiscal-year");


                public static StaticResourceDto Branch = new StaticResourceDto(ResourceOwner.Tenant, "Branch",
                    "Branch", "root.branch", "/branch");

                public static StaticResourceDto Partner = new StaticResourceDto(ResourceOwner.Tenant, "Partner",
                    "Partner", "root.partner", "/partner");


                public static StaticResourceDto User = new StaticResourceDto(ResourceOwner.Tenant, "User", "User", "root.user", "/user");

                public static StaticResourceDto AuditLog = new StaticResourceDto(ResourceOwner.Both, "Audit Log", "Audit Log", "root.audit-log", "/audit-log");


                public static StaticResourceDto Asset = new StaticResourceDto(ResourceOwner.Tenant, "Asset", "Asset", "root.asset", "/asset");

                public static StaticResourceDto Equity = new StaticResourceDto(ResourceOwner.Tenant, "Equity", "Equity", "root.equity", "/equity");

                public static StaticResourceDto Liability = new StaticResourceDto(ResourceOwner.Tenant, "Liability", "Liability", "root.liability", "/liability");

                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        AdministrationParent,

                        //Child
                        Company.AddParent(AdministrationParent),
                        CompanyProfile.AddParent(Company),
                        CompanySettings.AddParent(Company),
                        CompanyLogo.AddParent(Company),
                        CompanyFiscalYear.AddParent(Company),
                        Branch.AddParent(Company),
                        Partner.AddParent(Company),
                        User.AddParent(AdministrationParent),
                        AuditLog.AddParent(AdministrationParent),
                        //Asset.AddParent(AdministrationParent),
                        //Equity.AddParent(AdministrationParent),
                        //Liability.AddParent(AdministrationParent)
                        
                        //Crud Child
                        CompanyProfile.AddViewChildren(),
                        CompanyProfile.AddEditChildren(),
                        CompanySettings.AddViewChildren(),
                        CompanySettings.AddEditChildren()
                    };



                    list.AddRange(CompanyFiscalYear.AddCrudChildren());


                    list.AddRange(User.AddCrudChildren());
                    list.AddRange(Branch.AddCrudChildren());
                    list.AddRange(Partner.AddCrudChildren());
                    list.Add(AuditLog.AddViewChildren());
                    //list.AddRange(Asset.AddCrudChildren());
                    //list.AddRange(Equity.AddCrudChildren());

                    return list;
                }
            }

            public class Purchase
            {
                public static StaticResourceDto PurchaseParent =
                    new StaticResourceDto(ResourceOwner.Tenant, "Purchase", "Purchase", "root.purchase", null, false, 6);

                public static StaticResourceDto Supplier = new StaticResourceDto
                (ResourceOwner.Tenant, "Supplier", "Supplier", "root.supplier");

               
                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        PurchaseParent,

                        //Child
                        Supplier.AddParent(PurchaseParent),
                    };


                    //Crud Child
                    list.AddRange(Supplier.AddCrudChildren());


                    return list;
                }
            }

            public class Sales
            {

                public static StaticResourceDto SaleParent =
                    new StaticResourceDto(ResourceOwner.Tenant, "Sale", "Sale", "root.sale", null, false, 7);


                public static StaticResourceDto Customer =
                    new StaticResourceDto(ResourceOwner.Tenant, "Customer", "Customer", "root.customer");

              
                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        SaleParent,

                        //Child
                        Customer.AddParent(SaleParent),
                    };

                    //Crud Child
                    list.AddRange(Customer.AddCrudChildren());
                    
                    return list;
                }
            }

            public class Stock
            {
                public static StaticResourceDto StockParent =
                    new StaticResourceDto(ResourceOwner.Tenant, "Stock", "Stock", "root.stock", null, false, 8);


                public static StaticResourceDto Warehouse = new StaticResourceDto(ResourceOwner.Tenant,
                    "Warehouse", "Warehouse", "root.stock.warehouse");

                               public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        StockParent,

                        //Child
                        Warehouse.AddParent(StockParent),

                    };

                    //Crud Child
                    list.AddRange(Warehouse.AddCrudChildren());



                    return list;
                }
            }


            public class Hr
            {
                public static StaticResourceDto HrParent =
                    new StaticResourceDto(ResourceOwner.Tenant, "HR", "HR", "root.hr", null, false, 10);

                public static StaticResourceDto Employee = new StaticResourceDto
                    (ResourceOwner.Tenant, "Employee", "Employee", "root.hr.employee");

               
                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        HrParent,
                        Employee.AddParent(HrParent),
                    };

                    list.AddRange(Employee.AddCrudChildren());

                    return list;
                }
            }


            public static List<StaticResourceDto> GetAllPrivateResources()
            {
                var list = new List<StaticResourceDto>();
                list.AddRange(DataPermission.GetAll());
                list.AddRange(MultiTenant.GetAll());
                list.AddRange(Common.GetAll());
                list.AddRange(Landing.GetAll());
                list.AddRange(Administration.GetAll());
                list.AddRange(Purchase.GetAll());
                list.AddRange(Sales.GetAll());
                list.AddRange(Stock.GetAll());
                list.AddRange(Hr.GetAll());


                return list;
            }
        }


        public static List<StaticResourceDto> GetResources()
        {
            var list = new List<StaticResourceDto>();
            list.AddRange(Public.GetAllPublicResources());
            list.AddRange(Private.GetAllPrivateResources());

            return list;
        }
    }
}