using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Core.Enums;
using Project.Core.StaticResource;
using Project.Model;
using Project.ViewModel;
using Project.ViewModel.Session;
using Security.Models.Models;
using Security.Server.Managers;
using Security.Server.Stores;

namespace Security.Server.Providers.Sessions
{
    public class SessionProvider : ISessionProvider, IDisposable
    {
        private readonly BusinessDbContext _context;
        private readonly UserManager _userManager;

        public SessionProvider()
        {
            _context = new BusinessDbContext();
            _userManager = new UserManager(new AppUserStore(_context));

            var tenantId = HttpContext.Current.Request.Headers["TenantId"];
            _userManager.SetTenantId(string.IsNullOrWhiteSpace(tenantId) ? null : tenantId);
        }


        public SessionViewModel GetCurrentUserSession(string userId)
        {

            SessionViewModel viewModel = new SessionViewModel();

            var user = _userManager.FindById(userId);
            if (user != null)
            {
                viewModel.User.Id = user.Id;
                viewModel.User.FirstName = user.FirstName;
                viewModel.User.LastName = user.LastName;
                viewModel.User.BranchId = user.BranchId;
                viewModel.User.TenantId = user.TenantId;
                viewModel.User.TenantName = user.TenantName;
                viewModel.User.CompanyId = user.CompanyId;

                viewModel.User.Roles = _userManager.GetRoles(userId).ToList();


                var roles = _context.Roles.Where(x => x.TenantId == viewModel.User.TenantId && viewModel.User.Roles.Contains(x.Name)).ToList();
                var roleIds = roles.Select(x => x.Id).ToList();

                viewModel.User.Permissions = _context.Permissions.Where(x => x.TenantId == viewModel.User.TenantId && roleIds.Contains(x.RoleId))
                    .Include(x => x.Resource).ToList()
                    .ConvertAll(x => new PermissionViewModel()
                    {
                        RoleId = x.RoleId,
                        ResourceId = x.ResourceId,
                        ResourceState = x.Resource?.Route
                    });

                viewModel.User.IsCompanyAccessLevel = _context.Permissions.Any(x =>
                    x.TenantId == user.TenantId && roleIds.Contains(x.RoleId) &&
                    x.Resource.Name == StaticResource.Private.DataPermission.CompanyData.Name);

                var branch = _context.Branches.AsNoTracking().FirstOrDefault(x => x.Id == viewModel.User.BranchId);
                viewModel.User.WarehouseId = branch?.LinkedWarehouseId;
                viewModel.User.IsHeadOfficeBranch = branch != null && branch.Type == BranchType.HeadOffice;


                var tenant = _context.Tenants.AsNoTracking().FirstOrDefault(x => x.Id == viewModel.User.TenantId);
                if (tenant != null)
                {
                    viewModel.Subscription.NoOfShowroom = tenant.NoOfShowroom;
                    viewModel.Subscription.SubscriptionEndTime = tenant.SubscriptionEndTime;
                    viewModel.Subscription.IsInTrialPeriod = tenant.IsInTrialPeriod;
                    viewModel.Subscription.SubscriptionId = tenant.SubscriptionId;

                    var edition = _context.Editions.AsNoTracking().FirstOrDefault(x => x.Id == tenant.EditionId);
                    if (edition != null) viewModel.Subscription.WaitingDayAfterExpire = edition.WaitingDayAfterExpire;

                    viewModel.Subscription.HasAnySubscription = _context.Subscriptions.AsNoTracking()
                        .Any(x => x.TenantId == tenant.Id);

                    var subscription = _context.Subscriptions.AsNoTracking().FirstOrDefault(x => x.Id == tenant.SubscriptionId);
                    if (subscription != null)
                    {
                        viewModel.Subscription.IsAwaitingForPayment =
                            subscription.PaymentStatus == SubscriptionPaymentStatus.Pending ||
                            subscription.PaymentStatus == SubscriptionPaymentStatus.Unpaid;
                        viewModel.Subscription.IsAwaitingForActivation =
                            subscription.PaymentStatus == SubscriptionPaymentStatus.AwaitingConfirmation;
                        viewModel.Subscription.IsSubscriptionActivated =
                            subscription.PaymentStatus == SubscriptionPaymentStatus.Paid;

                        viewModel.Subscription.Package = subscription.Package;
                        viewModel.Subscription.Status = subscription.Status;
                        viewModel.Subscription.PaymentStatus = subscription.PaymentStatus;
                    }

                    viewModel.Application.IsDemoTenant = tenant.IsDemo;
                }

                var company = _context.Companies.AsNoTracking().FirstOrDefault(x => x.Id == viewModel.User.CompanyId);
                if (company != null) viewModel.Company = new CompanyViewModel(company);
                var companySetting = _context.CompanySettings.AsNoTracking().FirstOrDefault(x => x.CompanyId == viewModel.Company.Id);


                if (companySetting != null)
                {
                    viewModel.Company.Settings = new CompanySettingsViewModel(companySetting);
                    viewModel.Subscription.HasHostingAgreement = companySetting.HasYearlyHostingCharge;
                    viewModel.Subscription.HostingExpireDate = companySetting.HostingValidTill;
                }
            }

            return viewModel;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
