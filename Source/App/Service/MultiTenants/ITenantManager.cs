using System.Threading.Tasks;
using Project.Model;
using Project.ViewModel;

namespace Project.Service.MultiTenants
{
    public interface ITenantManager
    {
        string CreateTenant(TenantViewModel model);
        string CreateTenantCompany(string tenantId, string tenantName, string email, string phoneNumber);
        string CreateTenantCompanySettings(string tenantId, Company companyId);
        string CreateTenantHeadOfficeBranch(string tenantId, string companyId);
        string CreateTenantSubscription(TenantViewModel model, string tenantId, string companyId, string branchId,
            bool isLifeTimeSubscription);

        bool CreateSupplier(string tenantId, string companyId, string userId);
        
        Task RollbackAsync(string tenantId);
        Task DeleteTenantDependencyAsync(string tenantId);
        Task<Tenant> GetByTenancyName(string tenancyName);

        void ConfirmCompanyMobileNumber(SmsResponseModel smsResponseModel, string companyId);
        void UpdateSubscriptionId(string tenantId, string subscriptionId);



        #region Security

        bool IsEmailExist(string email);
        string CreateTenantAdminUser(TenantViewModel model, string tenantId, string companyId, string branchId, string adminUserId);
        void CreateTenantAdminPermission(string tenantId);
        Task ConfirmAdminMobileNumberAsync(SmsResponseModel smsResponseModel, string userId);


        /// <summary>
        /// Create roles and return the admin role id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="companyId"></param>
        string CreateTenantRole(string tenantId, string companyId);

        #endregion
    }
}