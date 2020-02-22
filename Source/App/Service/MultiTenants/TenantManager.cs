using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project.Core;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Core.Extensions.Framework;
using Project.Core.Handlers;
using Project.Core.Helpers;
using Project.Core.StaticResource;
using Project.Model;
using Project.Model.SeedData;
using Project.Repository;
using Project.Repository.MultiTenants;
using Project.Service.Sms;
using Project.ViewModel;
using Security.Models.Models;
using Security.Models.RequestModels;
using Security.Server.Managers;
using Security.Server.Repository;
using Security.Server.Service;

namespace Project.Service.MultiTenants
{
    public class TenantManager : ITenantManager
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICompanySettingsRepository _companySettingsRepository;

        private readonly IEditionRepository _editionRepository;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly ISmsService _smsService;
        private readonly ICustomerRepository _customerRepository;

        #region Security
        private readonly IUserService _userService;
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISecurityRepository<Role, string> _roleRepository;
        #endregion


        public TenantManager(ITenantRepository tenantRepository,
            ICompanyRepository companyRepository, 
            IBranchRepository branchRepository, 
            ISupplierRepository supplierRepository,
            IWarehouseRepository warehouseRepository,
            ISubscriptionRepository subscriptionRepository, 
            IEditionRepository editionRepository, 
            ICompanySettingsRepository companySettingsRepository, 
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            ISmsService smsService,
            ICustomerRepository customerRepository,

            IUserService userService,
            RoleManager roleManager,
            IEmployeeRepository employeeRepository,
            ISecurityRepository<Role, string> roleRepository)
        {
            _tenantRepository = tenantRepository;
            _companyRepository = companyRepository;
            _branchRepository = branchRepository;
            _supplierRepository = supplierRepository;
            _warehouseRepository = warehouseRepository;
            _subscriptionRepository = subscriptionRepository;
            _editionRepository = editionRepository;
            _companySettingsRepository = companySettingsRepository;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _smsService = smsService;

            _userService = userService;
            _roleManager = roleManager;
            _userManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<UserManager>();
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _customerRepository = customerRepository;

            var tenantId = HttpContext.Current?.Request.Headers["TenantId"];
            _userManager?.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
        }


        public string CreateTenant(TenantViewModel model)
        {
            var tenant = new Tenant()
            {
                Id = model.Id,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Modified = model.Modified,
                ModifiedBy = model.ModifiedBy,
                IsActive = true,
                Active = model.Active,

                EditionId = model.EditionId,
                ConnectionString = model.ConnectionString,
                Name = model.Name,
                TenancyName = model.TenancyName,
                Url = model.Url,
                IsInTrialPeriod = model.IsInTrialPeriod,
                SubscriptionEndTime = model.SubscriptionEndTime,
                LogoId = model.LogoId,
                LogoFilePath = model.LogoFilePath,
                NoOfShowroom = model.NoOfShowroom,
                Package = model.Package,
                BonusUserAccessCount = 1
            };

            _tenantRepository.CreateAsHost(tenant);
            _tenantRepository.Commit();

            return model.Id;
        }



        public string CreateTenantCompany(string tenantId, string tenantName, string email, string phoneNumber)
        {
            var id = Guid.NewGuid().ToString();
            Company company = new Company()
            {
                Active = true,
                Created = DateTime.Now,
                CreatedBy = null,
                Modified = null,
                ModifiedBy = null,
                Id = id,

                Name = tenantName,
                TenantId = tenantId,
                Email = email,
                IsEmailConfirmed = false,
                IsPhoneConfirmed = false,
                Phone = phoneNumber,
                Web = TenantHelper.GetTenantBaseUrl(tenantName.ToTenancyName())
            };

            _companyRepository.CreateAsHost(company);
            _companyRepository.Commit();

            CreateTenantCompanySettings(tenantId, company);

            return id;
        }


        public string CreateTenantCompanySettings(string tenantId, Company company)
        {
            var id = Guid.NewGuid().ToString();
            CompanySetting companySetting = new CompanySetting
            {
                Active = true,
                Created = DateTime.Now,
                CreatedBy = null,
                Modified = null,
                ModifiedBy = null,
                Id = id,

                CustomerInvoiceTermsAndConditions = null,
                SaleVatPercentage = 0,
                IsUseDefaultSettings = true,
                IsEnableEmailNotification = true,
                IsSendEmailToAdminForTransaction = true,

                IsEnableSmsNotification = true,
                IsSendSmsToCustomerForTransaction = true,

                HostingValidTill = null,
                PoweredBy = AppConst.PoweredBy,

                TenantId = tenantId,
                CompanyId = company.Id,
            };

            if (companySetting.IsUseDefaultSettings)
            {
                companySetting.EmailSenderDisplayName = company?.Name;
                companySetting.NotificationSenderEmail = company?.Email;
            }

            _companySettingsRepository.CreateAsHost(companySetting);
            _companySettingsRepository.Commit();

            return id;
        }



        public string CreateTenantHeadOfficeBranch(string tenantId, string companyId)
        {
            var warehouseId = CreateTenantHeadOfficeWarehouse(tenantId, companyId);

            var id = Guid.NewGuid().ToString();
            Branch branch = new Branch()
            {
                Active = true,
                Created = DateTime.Now,
                CreatedBy = null,
                Modified = DateTime.Now,
                ModifiedBy = null,
                Id = id,

                Name = StaticBranch.HeadOffice,
                Code = "HO",
                Type = BranchType.HeadOffice,
                LinkedWarehouseId = warehouseId,
                CompanyId = companyId,
                TenantId = tenantId,
            };

            _branchRepository.CreateTenantHeadOfficeBranch(branch);
            _branchRepository.Commit();

            return id;
        }


        public string CreateTenantHeadOfficeWarehouse(string tenantId, string companyId)
        {
            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                CreatedBy = null,
                ModifiedBy = null,
                Active = true,
                Code = "HO",
                Name = StaticWarehouse.HeadOffice,
                Type = WarehouseType.HeadOffice,
                TenantId = tenantId,
                CompanyId = companyId
            };


            _warehouseRepository.CreateTenantWarehouse(warehouse);

            _warehouseRepository.Commit();

            return warehouse.Id;
        }

        public string CreateTenantSubscription(TenantViewModel model, string tenantId, string companyId,
            string branchId, bool isLifeTimeSubscription)
        {
            var status = isLifeTimeSubscription ? SubscriptionStatus.Active : SubscriptionStatus.AwaitingPayment;
            var paymentStatus = isLifeTimeSubscription ? SubscriptionPaymentStatus.Paid : SubscriptionPaymentStatus.Unpaid;
            bool isPaymentCompleted = false;

            var edition = _editionRepository.AsNoTracking().SingleOrDefault(x => x.Id == model.EditionId);
            if (edition != null && edition.Name == StaticEdition.Trial)
            {
                status = SubscriptionStatus.Trial;
                paymentStatus = SubscriptionPaymentStatus.Paid;
                isPaymentCompleted = true;
            }

            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Modified = model.Modified,
                ModifiedBy = model.ModifiedBy,
                Active = model.Active,

                TenantId = tenantId,
                CompanyId = companyId,

                EditionId = model.EditionId,

                Package = model.Package,
                PackageMonthlyPrice = model.PackageMonthlyPrice,
                PackageMonth = model.PackageMonth,
                NoOfShowroom = model.NoOfShowroom,
                Price = model.Price,
                PackageDiscountPercentage = model.PackageDiscountPercentage,
                PackageDiscountAmount = model.PackageDiscountAmount,
                PackageCharge = model.PackageCharge,

                ExpireOn = model.SubscriptionEndTime,
                RenewedOn = null,
                Status = status,
                PaymentStatus = paymentStatus,
                IsPaymentCompleted = isPaymentCompleted
            };

            _subscriptionRepository.CreateAsHost(subscription);
            _subscriptionRepository.Commit();

            return subscription.Id;
        }


        public bool CreateSupplier(string tenantId, string companyId, string userId)
        {            
            var suppliers = new List<Supplier>();

            var list = StaticSupplier.GetSuppliers();
            foreach (var supplier in list)
            {
                suppliers.Add(new Supplier
                {
                    Id = Guid.NewGuid().ToString(),
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    Active = true,

                    Code = supplier,
                    Name = supplier,
                    Type = SupplierType.Company,
                    Phone = null,
                    TenantId = tenantId,
                    CompanyId = companyId
                });
            }
            _supplierRepository.CreateTenantSupplier(suppliers);

            return _supplierRepository.Commit();
        }

        
        public async Task RollbackAsync(string tenantId)
        {
            var subscriptions = await _subscriptionRepository.GetAll().Where(x => x.TenantId == tenantId).ToListAsync();
            _subscriptionRepository.Delete(subscriptions);
            await _subscriptionRepository.CommitAsync();


            var companySettings = await _companySettingsRepository.GetAll().Where(x => x.TenantId == tenantId).ToListAsync();
            _companySettingsRepository.Delete(companySettings);
            await _companySettingsRepository.CommitAsync();

            var companies = await _companyRepository.GetAll().Where(x => x.TenantId == tenantId).ToListAsync();
            _companyRepository.Delete(companies);
            await _companyRepository.CommitAsync();

            var branches = await _branchRepository.GetAll().Where(x => x.TenantId == tenantId).ToListAsync();
            _branchRepository.Delete(branches);
            await _branchRepository.CommitAsync();

            var warehouses = await _warehouseRepository.GetAll().Where(x => x.TenantId == tenantId).ToListAsync();
            _warehouseRepository.Delete(warehouses);
            await _warehouseRepository.CommitAsync();

            #region Security

            var users = _userService.GetUsersByTenantId(tenantId);

            foreach (var user in users)
                await _userService.DeleteUserAsync(user);

            #endregion


            var tenants = await _tenantRepository.GetAll().Where(x => x.Id == tenantId).ToListAsync();
            _tenantRepository.Delete(tenants);
            await _tenantRepository.CommitAsync();
        }


        public async Task DeleteTenantDependencyAsync(string tenantId)
        {
            var companies = await _companyRepository.GetAll().Where(x => x.TenantId == tenantId).ToListAsync();
            _companyRepository.TrashAll(companies);

            var payments = await _subscriptionPaymentRepository.GetAll()
                .Where(x => x.TenantId == tenantId && x.PaymentStatus != SubscriptionPaymentStatus.Paid &&
                            x.PaymentStatus != SubscriptionPaymentStatus.AwaitingConfirmation)
                .ToListAsync();
            _subscriptionPaymentRepository.TrashAll(payments);

            var subscriptions = await _subscriptionRepository.GetAll().Where(x => x.TenantId == tenantId).ToListAsync();
            _subscriptionRepository.TrashAll(subscriptions);

            var branches = await _branchRepository.GetAll().Where(x => x.TenantId == tenantId).ToListAsync();
            _branchRepository.TrashAll(branches);

            await _subscriptionRepository.CommitAsync();
            await _subscriptionPaymentRepository.CommitAsync();
            await _companyRepository.CommitAsync();
            await _branchRepository.CommitAsync();

            #region Security

            var users = _userService.GetUsersByTenantId(tenantId);

            foreach (var user in users)
            {
                await _userService.DeleteUserAsync(user);
            }

            #endregion


        }

        public async Task<Tenant> GetByTenancyName(string tenancyName)
        {
            return await _tenantRepository.GetAll().AsNoTracking()
                .FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenancyName.ToLower());
        }

        public void ConfirmCompanyMobileNumber(SmsResponseModel smsResponseModel, string companyId)
        {
            try
            {
                //var deliveryResponseModel = _smsService.GetDeliveryStatusUsingApi(smsResponseModel.ResponseId);
                //if (deliveryResponseModel?.IsDeliveredSuccess != null && deliveryResponseModel.IsDeliveredSuccess.GetValueOrDefault())
                //{
                var company = _companyRepository.GetById(companyId);
                if (smsResponseModel.RecipientNumbers.Contains(company.Phone))
                {
                    company.IsPhoneConfirmed = true;
                    _companyRepository.EditAsHost(company);
                    _companyRepository.Commit();
                }
                //}
            }
            catch (Exception e)
            {
                // ignored
            }
        }


        public void UpdateSubscriptionId(string tenantId, string subscriptionId)
        {
            var tenant = _tenantRepository.GetById(tenantId);
            tenant.SubscriptionId = subscriptionId;

            _tenantRepository.EditAsHost(tenant);
            _tenantRepository.Commit();
        }

        #region Security


        public bool IsEmailExist(string email)
        {
            return _userService.IsEmailExist(email);
        }


        public string CreateTenantAdminUser(TenantViewModel model, string tenantId, string companyId, string branchId, string adminRoleId)
        {
            string employeeId = CreateTenantAdminEmployee(model, tenantId, companyId, branchId);

            var userId = Guid.NewGuid().ToString();

            var adminUser = new UserCreateRequestModel
            {
                Id = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = model.PasswordHash,
                RetypePassword = model.RetypePassword,
                SecurityStamp = Guid.NewGuid().ToString(),

                RoleId = !string.IsNullOrWhiteSpace(adminRoleId) ? adminRoleId : _roleManager.GetByName(StaticRoles.Admin.Name)?.Id,

                //EmailConfirmationCode = emailConfirmationCode,
                //PhoneConfirmationCode = phoneNumberConfirmationCode,

                TenantId = tenantId,
                TenantName = model.TenancyName,
                CompanyId = companyId,
                BranchId = branchId,
                EmployeeId = employeeId,

                IsActive = true,
                CreatedBy = null,
                Created = DateTime.Now,
                IsShouldChangedPasswordOnNextLogin = false,

            };

            _userService.CreateTenantAdminUser(adminUser);

            var emailCode = _userManager.GenerateEmailConfirmationToken<User, string>(userId);
            var phoneCode = _userManager.GenerateChangePhoneNumberToken(userId, model.PhoneNumber);

            var user = _userManager.FindById(userId);
            user.EmailConfirmationCode = emailCode;
            user.PhoneConfirmationCode = phoneCode;

            _userService.Update(user);

            var task = Task.Run(async () => await _userService.SendEmailConfirmationLinkAsync(userId, user.FullName(), model.Email, emailCode));
            task.Wait();

            return model.Id;
        }

        private string CreateTenantAdminEmployee(TenantViewModel model, string tenantId, string companyId, string branchId)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid().ToString(),
                Code = "EM001",
                Name = model.FirstName + " " + model.LastName,
                Email = model.Email,
                Phone = model.PhoneNumber,
                Type = EmployeeType.FullTime,
                DateOfBirth = null,

                TenantId = tenantId,
                CompanyId = companyId,
                BranchId = branchId,

                Active = true,
                CreatedBy = null,
                Created = DateTime.Now,
                ModifiedBy = null,
                Modified = DateTime.Now
            };

            _employeeRepository.CreateTenantAdminEmployee(employee);
            _employeeRepository.Commit();

            return employee.Id;
        }



        public async Task ConfirmAdminMobileNumberAsync(SmsResponseModel smsResponseModel, string userId)
        {
            try
            {
                //var deliveryResponseModel = _smsService.GetDeliveryStatusUsingApi(smsResponseModel.ResponseId);
                //if (deliveryResponseModel?.IsDeliveredSuccess != null && deliveryResponseModel.IsDeliveredSuccess.GetValueOrDefault())
                //{
                var user = await _userManager.FindByIdAsync(userId);
                if (smsResponseModel.RecipientNumbers.Contains(user.PhoneNumber))
                {
                    user.PhoneNumberConfirmed = true;
                    user.PhoneConfirmationCode = null;
                    user.PasswordChangeConfirmationCodeExpireTime = null;
                    await _userManager.UpdateAsync(user);
                }
                //}
            }
            catch (Exception e)
            {
                // ignored
            }
        }


        /// <summary>
        /// Create roles and return the admin role id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="companyId"></param>
        public string CreateTenantRole(string tenantId, string companyId)
        {
            var roles = StaticRoles.GetRoles();

            if (_roleRepository.GetAll().FilterTenant(tenantId).Any()) return null;

            string adminRoleId = Guid.NewGuid().ToString();
            foreach (var role in roles)
            {
                var id = role.Name == StaticRoles.Admin.Name ? adminRoleId : Guid.NewGuid().ToString();
                _roleRepository.Create(new Role
                {
                    Id = id,
                    Name = role.Name,
                    DisplayName = role.DisplayName,
                    AccessLevel = role.AccessLevel,
                    TenantId = tenantId,
                    CompanyId = companyId
                });
            }

            _roleRepository.Save();

            return adminRoleId;
        }


        public void CreateTenantAdminPermission(string tenantId)
        {
            PermissionBuilder.Build(BusinessDbContext.Create(), tenantId);
        }

        #endregion

    }
}

