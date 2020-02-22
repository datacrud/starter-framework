using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Core.Session;
using Project.Core.StaticResource;
using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IMonthlyEmailNotficationBalanceService : IHaveTenantIdCompanyIdBranchIdServiceBase<MonthlyEmailBalance, MonthlyEmailBalanceViewModel>
    {
        void Update(DateTime today);
    }

    public class MonthlyEmailNotficationBalanceService : HaveTenantIdCompanyIdBranchIdServiceBase<MonthlyEmailBalance, MonthlyEmailBalanceViewModel>, IMonthlyEmailNotficationBalanceService
    {
        private readonly IMonthlyEmailNotficationBalanceRepository _repository;
        private readonly IEmailNotificationLogRepository _emailNotificationLogRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFeatureService _featureService;

        public MonthlyEmailNotficationBalanceService(IMonthlyEmailNotficationBalanceRepository repository, IEmailNotificationLogRepository emailNotificationLogRepository, ISubscriptionRepository subscriptionRepository, IFeatureService featureService) : base(repository)
        {
            _repository = repository;
            _emailNotificationLogRepository = emailNotificationLogRepository;
            _subscriptionRepository = subscriptionRepository;
            _featureService = featureService;
        }

        public void Update(DateTime today)
        {
            var month = GetMonth(today.Month);

            var monthlyEmailNotficationBalance = _repository.GetAllAsTenant()
                .FirstOrDefault(x => x.MonthStartDate <= today && x.MonthEndDate >= today && x.Month == month);
            if (monthlyEmailNotficationBalance == null)
            {
                CreateCurrentMonthEmailBalance(DateTime.Today);

                monthlyEmailNotficationBalance = _repository.GetAllAsTenant()
                    .FirstOrDefault(x => x.MonthStartDate <= today && x.MonthEndDate >= today && x.Month == month);
            }

            if(monthlyEmailNotficationBalance == null) return;


            var send = _emailNotificationLogRepository.AsNoTracking().Count();

            var value = _featureService.GetEditionFeatureValue(monthlyEmailNotficationBalance.TenantId,
                StaticFeature.MonthlyEmailNotification.Name);

            monthlyEmailNotficationBalance.TotalSubscribeEmail = Convert.ToInt32(value);
            monthlyEmailNotficationBalance.TotalSendEmail = send;
            monthlyEmailNotficationBalance.TotalRemainingEmail =
                monthlyEmailNotficationBalance.TotalSubscribeEmail - monthlyEmailNotficationBalance.TotalSendEmail;

            _repository.EditAsTenant(monthlyEmailNotficationBalance);

            _repository.Commit();
        }

        private void CreateCurrentMonthEmailBalance(DateTime today)
        {
            AppSession appSession = new AppSession();

            //if (!_repository.GetAllAsTenant().Any())
            //{                

            //    var status = Convert.ToInt32(SubscriptionStatus.Active);
            //    var subscription =  _subscriptionRepository.GetAllActiveAsNoTracking()
            //        .Where(x => x.IsPaymentCompleted && x.Status == status.ToString()).OrderBy(x => x.ExpireOn)
            //        .FirstOrDefault();

            //    if (subscription != null)
            //    {
            //        var monthDiff = Math.Abs(((subscription.Modified.Year - today.Year) * 12) +
            //                                 subscription.Modified.Month - today.Month);

            //        var monthStartDate = subscription.Modified.Date;
            //        var monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);

            //        for (int i = 0; i < monthDiff; i++)
            //        {
            //            var featureViewModel = _featureService.GetAll()
            //                .FirstOrDefault(x => x.IsEditionFeature && x.EditionId == subscription.EditionId &&
            //                                     x.Name == StaticFeature.MonthlyEmailNotification.ToString());

            //            MonthlyEmailBalance monthlyEmailNotficationBalance = new MonthlyEmailBalance
            //            {
            //                Id = Guid.NewGuid().ToString(),
            //                Active = true,
            //                CreatedBy = session.UserId,
            //                Created = DateTime.Now,
            //                ModifiedBy = session.UserId,
            //                Modified = DateTime.Now,
            //                TenantId = session.TenantId,
            //                CompanyId = session.CompanyId,

            //                //SubscriptionId
            //                EditionId = subscription.EditionId,
            //                MonthStartDate = monthStartDate,
            //                MonthEndDate = monthEndDate,
            //                Month = GetMonth(monthStartDate.Month),
            //                TotalSubscribeEmail = Convert.ToInt32(featureViewModel?.Value),
            //                TotalSendEmail = 0,
            //                TotalRemainingEmail = 0,
            //                CarryForwardedEmailFromLastMonth = 0,
            //                IsAllowSendEmailFromCarryForward = false
            //            };

            //            _repository.AddAsTenant(monthlyEmailNotficationBalance);

            //            monthStartDate = monthEndDate.AddDays(1);
            //            monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);
            //        }
                    
            //        _repository.Commit();
            //    }
            //}
            //else
            {
                var monthStartDate = new DateTime(today.Year, today.Month, 1);
                var monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);

                var subscription = _subscriptionRepository
                    .AsNoTracking()
                    .Where(x => x.IsPaymentCompleted && x.Status == SubscriptionStatus.Active && x.ExpireOn >= today)
                    .OrderBy(x => x.ExpireOn).FirstOrDefault();

                if (subscription != null)
                {                    
                    MonthlyEmailBalance monthlyEmailBalance = new MonthlyEmailBalance
                    {
                        Id = Guid.NewGuid().ToString(),
                        Active = true,
                        CreatedBy = appSession.UserId,
                        Created = DateTime.Now,
                        ModifiedBy = appSession.UserId,
                        Modified = DateTime.Now,
                        TenantId = appSession.TenantId,
                        CompanyId = appSession.CompanyId,

                        EditionId = subscription.EditionId,
                        MonthStartDate = monthStartDate,
                        MonthEndDate = monthEndDate,
                        Month = GetMonth(monthStartDate.Month),
                        TotalSubscribeEmail = 0,
                        TotalSendEmail = 0,
                        TotalRemainingEmail = 0,
                        CarryForwardedEmailFromLastMonth = 0,
                        IsAllowSendEmailFromCarryForward = false
                    };

                    _repository.AddAsTenant(monthlyEmailBalance);

                    _repository.Commit();
                }
            }
        }

        private Month GetMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return Month.January;
                case 2:
                    return Month.February;
                case 3:
                    return Month.March;
                case 4:
                    return Month.April;
                case 5:
                    return Month.May;
                case 6:
                    return Month.June;
                case 7:
                    return Month.July;
                case 8:
                    return Month.August;
                case 9:
                    return Month.September;
                case 10:
                    return Month.October;
                case 11:
                    return Month.November;
                case 12:
                    return Month.December;
            }

            return 0;
        }
    }
}
