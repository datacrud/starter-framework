using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Project.Core.Enums;
using Project.Model;
using Project.Repository;
using Project.RequestModel;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface ISubscriptionService : IHaveTenantIdCompanyIdServiceBase<Subscription, SubscriptionViewModel>
    {
        List<SubscriptionViewModel> GetTenantSubscriptions(string tenantId);
        bool UpdatePaymentStatus(string subscriptionId, SubscriptionPaymentStatus paymentStatus);
        bool UpdateSubscriptionStatus(string subscriptionId, bool isUpdatePaymentStatusSuccess);
        bool RevertPreviousSubscriptionRenewedOn(string tenantId);
    }

    public class SubscriptionService : HaveTenantIdCompanyIdServiceBase<Subscription, SubscriptionViewModel>, ISubscriptionService
    {
        private readonly ISubscriptionRepository _repository;
        private readonly ISubscriptionPaymentService _subscriptionPaymentService;
        private readonly ITenantService _tenantService;

        public SubscriptionService(ISubscriptionRepository repository, 
            ISubscriptionPaymentService subscriptionPaymentService, 
            ITenantService tenantService) : base(repository)
        {
            _repository = repository;
            _subscriptionPaymentService = subscriptionPaymentService;
            _tenantService = tenantService;
        }

        public override ResponseModel<SubscriptionViewModel> GetAll(RequestModelBase<Subscription> requestModel)
        {
            var queryable = GetPagingQuery(Repository.GetAll().Include(x=> x.Tenant), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<SubscriptionViewModel>(entities, Repository.GetAll().Count());
            
            return response;
        }


        public override ResponseModel<SubscriptionViewModel> GetAllAsTenant(HaveTenantIdCompanyIdRequestModelBase<Subscription> requestModel)
        {
            var queryable = GetPagingQuery(_repository.GetAll().Include(x => x.Edition).Include(x=> x.Tenant), requestModel);
            var entities = GetEntries(queryable);

            var response = new ResponseModel<SubscriptionViewModel>(entities, _repository.GetAll().Count());

            return response;
        }

        public override List<SubscriptionViewModel> GetAllAsTenant(string tenantId = null)
        {
            var queryable = _repository.GetAllAsTenant().Include(x => x.Edition);
            var entities = GetEntries(queryable);
            return entities;
        }



        public List<SubscriptionViewModel> GetTenantSubscriptions(string tenantId)
        {
            var queryable = _repository.GetAllAsTenant().Where(x => x.TenantId == tenantId).Include(x => x.Edition);
            var entities = GetEntries(queryable);
            return entities;
        }

        public bool UpdatePaymentStatus(string subscriptionId, SubscriptionPaymentStatus paymentStatus)
        {
            var subscription = _repository.GetById(subscriptionId);
            subscription.PaymentStatus = paymentStatus;
            subscription.IsPaymentCompleted = subscription.PaymentStatus == SubscriptionPaymentStatus.Paid;

            _repository.EditAsTenant(subscription);
            return _repository.Commit();
        }

        public bool UpdateSubscriptionStatus(string subscriptionId, bool isUpdatePaymentStatusSuccess)
        {
            var subscription = _repository.GetById(subscriptionId);
            subscription.PaymentStatus =
                isUpdatePaymentStatusSuccess ? SubscriptionPaymentStatus.Paid : SubscriptionPaymentStatus.Rejected;
            subscription.IsPaymentCompleted = isUpdatePaymentStatusSuccess;
            subscription.Status = isUpdatePaymentStatusSuccess
                ? SubscriptionStatus.Active
                : SubscriptionStatus.Inactive;

            _repository.EditAsTenant(subscription);

            return _repository.Commit();
        }

        public bool RevertPreviousSubscriptionRenewedOn(string tenantId)
        {
            var subscription = _repository.AsNoTracking()
                .Where(x => x.TenantId == tenantId).OrderByDescending(x => x.ExpireOn)
                .FirstOrDefault();

            if (subscription != null)
            {
                subscription.RenewedOn = null;
                subscription.TenantId = tenantId;
                _repository.EditAsHost(subscription);
            }


            return _repository.Commit();
        }


        public override bool Trash(string id)
        {
            var subscription = _repository.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (subscription != null)
            {
                var tenantId = subscription.TenantId;

                using (var scope = new TransactionScope())
                {
                    _subscriptionPaymentService.DeleteSubscriptionPayments(id);

                    base.Trash(id);

                    RevertPreviousSubscriptionRenewedOn(tenantId);

                    _tenantService.RevertTenantSubscriptionInfo(tenantId);

                    scope.Complete();
                }

                return true;
            }


            return false;
        }


        public override bool Delete(string id)
        {
            var subscription = _repository.AsNoTracking().FirstOrDefault(x=> x.Id == id);
            if (subscription != null)
            {
                var tenantId = subscription.TenantId;

                using (var scope = new TransactionScope())
                {
                    _subscriptionPaymentService.DeleteSubscriptionPayments(id);

                    base.Delete(id);

                    RevertPreviousSubscriptionRenewedOn(tenantId);

                    _tenantService.RevertTenantSubscriptionInfo(tenantId);

                    scope.Complete();
                }

                return true;
            }


            return false;
        }
    }
}