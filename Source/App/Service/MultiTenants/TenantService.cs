using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Core.Helpers;
using Project.Model;
using Project.Repository;
using Project.RequestModel;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface ITenantService : IServiceBase<Tenant, TenantViewModel>
    {
        bool IsTenantExist(string tenancyName);
        bool UpdateTenantSubscriptionInfo(string tenantId, string editionId, string subscriptionId);
        bool RevertTenantSubscriptionInfo(string tenantId);
        bool IsReservedName(string tenancyName);
        TenantCurrentSubscriptionInfoViewModel GetTenantCurrentSubscription(string tenantId);
    }

    public class TenantService : ServiceBase<Tenant, TenantViewModel>, ITenantService
    {
        private readonly ITenantRepository _repository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public TenantService(ITenantRepository repository, ISubscriptionRepository subscriptionRepository) : base(repository)
        {
            _repository = repository;
            _subscriptionRepository = subscriptionRepository;
        }

        public override ResponseModel<TenantViewModel> GetAll(RequestModelBase<Tenant> requestModel)
        {
            var queryable = GetPagingQuery(_repository.GetAllAsTenant().Include(x => x.Edition), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<TenantViewModel>(entities, _repository.GetAllAsTenant().Count());

            return response;
        }

        public bool IsTenantExist(string tenancyName)
        {
            var tenant = _repository.AsNoTracking()
                .FirstOrDefault(x => x.TenancyName == tenancyName && x.Active && !x.IsDeleted);
            if (tenant != null) return true;

            return false;
        }


        public bool UpdateTenantSubscriptionInfo(string tenantId, string editionId, string subscriptionId)
        {
            var tenant = _repository.GetById(tenantId);

            var currentSubscription = _subscriptionRepository.AsNoTracking().SingleOrDefault(x => x.Id == subscriptionId);

            var subscriptions = _subscriptionRepository.AsNoTracking()
                .Where(x => x.TenantId == tenantId && x.ExpireOn > DateTime.Today).ToList();

            //int CurrentSubscriptionEquivalentDays(int convertibleDays, int convertibleAmount)
            //{
            //    return convertibleDays / (convertibleAmount * currentSubscription.NoOfShowroom) + 1;
            //}

            //int GetPackageDays(SubscriptionPackage package)
            //{
            //    int packageDays = 0;

            //    if (package == SubscriptionPackage.Trial) packageDays = 14;
            //    else if (package == SubscriptionPackage.Monthly) packageDays = 30;
            //    else if (package == SubscriptionPackage.Quarter) packageDays = 365 / 4;
            //    else if (package == SubscriptionPackage.HalfYearly) packageDays = 365 / 2;                
            //    else if (package == SubscriptionPackage.Annual) packageDays = 365;

            //    return packageDays;
            //}

            var days = 0;
            foreach (var subscription in subscriptions)
            {
                if (subscription.ExpireOn.HasValue)
                {
                    var convertibleDays = (subscription.ExpireOn.GetValueOrDefault() - DateTime.Today).Days;

                    days += (convertibleDays * subscription.NoOfShowroom) / currentSubscription.NoOfShowroom + 1;
                }
            }
            tenant.SubscriptionEndTime = DateTime.Today.AddDays(days + 1).AddSeconds(-1);

            if (currentSubscription != null)
            {
                tenant.SubscriptionEndTime = currentSubscription.Package == SubscriptionPackage.LifeTime
                    ? null
                    : tenant.SubscriptionEndTime;

                tenant.SubscriptionId = currentSubscription.Id;
                tenant.NoOfShowroom = currentSubscription.NoOfShowroom;
                tenant.Package = currentSubscription.Package;
            }

            tenant.EditionId = editionId;
            tenant.IsInTrialPeriod = false;


            _repository.EditAsTenant(tenant);

            return _repository.Commit();
        }


        public bool RevertTenantSubscriptionInfo(string tenantId)
        {
            var date = DateTime.Today;
            var tenant = _repository.GetById(tenantId);

            var subscription = _subscriptionRepository.AsNoTracking()
                .Where(x => x.TenantId == tenantId).OrderByDescending(x => x.ExpireOn)
                .FirstOrDefault();

            tenant.SubscriptionEndTime = subscription?.ExpireOn;
            if (subscription != null)
            {
                tenant.SubscriptionEndTime = subscription.Package == SubscriptionPackage.LifeTime
                    ? null
                    : tenant.SubscriptionEndTime;

                tenant.IsInTrialPeriod = subscription.Status == SubscriptionStatus.Trial;
                tenant.SubscriptionId = subscription.Id;
                tenant.NoOfShowroom = subscription.NoOfShowroom;
                tenant.Package = subscription.Package;
            }

            _repository.EditAsTenant(tenant);

            return _repository.Commit();
        }

        public bool IsReservedName(string tenancyName)
        {
            var reserveNames = TenantHelper.GetReservedNames();

            return reserveNames.Any(x => x.ToLower() == tenancyName.ToLower());
        }

        public TenantCurrentSubscriptionInfoViewModel GetTenantCurrentSubscription(string tenantId)
        {
            TenantCurrentSubscriptionInfoViewModel viewModel = null;

            var tenant = _repository.GetAll().Include(x => x.Edition).FirstOrDefault(x => x.Id == tenantId);
            if (tenant != null)
            {
                viewModel = new TenantCurrentSubscriptionInfoViewModel()
                {
                    NoOfShowroom = tenant.NoOfShowroom,
                    SubscriptionEndTime = tenant.SubscriptionEndTime,
                    SubscriptionId = tenant.SubscriptionId,
                    Package = tenant.Package,
                    EditionName = tenant.Edition?.DisplayName
                };
            }

            return viewModel;
        }
    }


}
