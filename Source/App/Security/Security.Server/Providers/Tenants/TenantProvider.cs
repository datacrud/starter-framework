using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Project.Core.Enums;
using Project.Model;
using Project.ViewModel;

namespace Security.Server.Providers
{
    public class TenantProvider : ITenantProvider, IDisposable
    {
        private readonly BusinessDbContext _context;

        public TenantProvider()
        {
            _context = new BusinessDbContext();
        }

        public IQueryable<Tenant> GetAll()
        {
            return _context.Tenants.AsQueryable();
        }

        public async Task<Tenant> GetTenantByNameAsync(string tenancyName)
        {
            return await _context.Tenants.AsNoTracking().FirstOrDefaultAsync(x=> x.TenancyName == tenancyName);
        }

        public async Task<Tenant> GetTenantAsync(string tenantId)
        {
            return await _context.Tenants.FindAsync(tenantId);
        }

        public async Task<Edition> GetEditionAsync(string editionId)
        {
            return await _context.Editions.FindAsync(editionId);
        }

        public async Task<List<Subscription>> GetActiveSubscriptionsAsync(string tenantId)
        {
            return await _context.Subscriptions.Where(x => x.TenantId == tenantId && x.Active && x.ExpireOn.GetValueOrDefault().Date >= DateTime.Today)
                .Include(x => x.Edition)
                .OrderBy(x => x.ExpireOn)
                .ToListAsync();
        }

        public async Task<bool> AnyUnpaidSubscription(string tenantId)
        {
            return await _context.Subscriptions
                .AnyAsync(x =>
                    x.TenantId == tenantId && x.Active && x.ExpireOn >= DateTime.Now && !x.IsPaymentCompleted);

        }

        public async Task<bool> AllUnpaidSubscription(string tenantId)
        {
            var subscriptions = await _context.Subscriptions
                .Where(x => x.TenantId == tenantId && x.Active && x.ExpireOn >= DateTime.Now && !x.IsPaymentCompleted)
                .ToListAsync();

            return subscriptions.All(x => !x.IsPaymentCompleted);
        }

        public async Task<string> SubscriptionLastPaymentStatus(string tenantId)
        {
            string status = "";

            var subscription = await _context.Subscriptions
                .Where(x => x.TenantId == tenantId && x.Active && x.ExpireOn >= DateTime.Now && !x.IsPaymentCompleted)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            if (subscription != null)
                status = subscription.PaymentStatus.ToString();
            return status;
        }

        public async Task<bool> AnyActiveSubscription(string tenantId)
        {
            return await _context.Subscriptions.AnyAsync(x => x.Active && x.TenantId == tenantId);
        }

        public async Task SaveLoginAttempt(bool isSuccess, string userName, string error, string tenantId, string companyId, string userId)
        {
            _context.LoginAttempts.Add(new LoginAttempt()
            {
                Id = Guid.NewGuid().ToString(),
                Active = true,
                Date = DateTime.Now,

                Status = isSuccess ? LoginAttemptStatus.Success : LoginAttemptStatus.Failed,
                Username = userName,
                Error = error,

                IpAddress = HttpContext.Current != null
                    ? HttpContext.Current.Request.UserHostAddress
                    : null,

                Created = DateTime.Now,
                CreatedBy = userId,

                TenantId = tenantId,
                CompanyId = companyId,

                DeviceInfo = HttpContext.Current != null ? HttpContext.Current.Request.UserAgent : null,

            });

            await _context.SaveChangesAsync();
        }

        public async Task<Company> GetTenantCompanyAsync(string tenantId)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.TenantId == tenantId);
        }

        public async Task<CompanySettingsAccountInfoViewModel> GetCompanySettings(string tenantId)
        {
            var companySettings = await _context.CompanySettings.FirstOrDefaultAsync(x => x.TenantId == tenantId);

            return new CompanySettingsAccountInfoViewModel(companySettings);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}