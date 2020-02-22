using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Model;
using Project.ViewModel;

namespace Security.Server.Providers
{
    public interface ITenantProvider
    {
        IQueryable<Tenant> GetAll();

        Task<Tenant> GetTenantByNameAsync(string tenancyName);
        Task<Tenant> GetTenantAsync(string tenantId);
        Task<Edition> GetEditionAsync(string editionId);
        Task<List<Subscription>> GetActiveSubscriptionsAsync(string tenantId);
        Task<bool> AnyUnpaidSubscription(string tenantId);
        Task<bool> AllUnpaidSubscription(string tenantId);
        Task<string> SubscriptionLastPaymentStatus(string tenantId);
        Task<bool> AnyActiveSubscription(string tenantId);
        Task SaveLoginAttempt(bool isSuccess, string userName, string error, string tenantId, string companyId, string userId);
        Task<Company> GetTenantCompanyAsync(string tenantId);
        Task<CompanySettingsAccountInfoViewModel> GetCompanySettings(string tenantId);
    }
}