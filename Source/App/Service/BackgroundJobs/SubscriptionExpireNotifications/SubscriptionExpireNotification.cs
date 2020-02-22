using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Project.Core;
using Project.Core.Email;
using Project.Core.Extensions;
using Project.Core.Helpers;
using Project.Core.StaticResource;
using Project.Model;
using Project.Service.Sms;

namespace Project.Service.BackgroundJobs.SubscriptionExpireNotifications
{
    public class SubscriptionExpireNotification : ISubscriptionExpireNotification
    {
        private readonly ISmsService _smsService;
        private readonly IEmailSender _emailSender;

        public SubscriptionExpireNotification(ISmsService smsService, IEmailSender emailSender)
        {
            _smsService = smsService;
            _emailSender = emailSender;
        }



        public async Task ExecuteNotify()
        {
            using (var db = new BusinessDbContext())
            {
                var tenants = await db.Tenants.AsNoTracking()
                    .Where(x => x.IsActive && x.Active && !x.IsDeleted && x.SubscriptionEndTime >= DateTime.Today).ToListAsync();

                foreach (var tenant in tenants)
                {
                    var company = await db.Companies.AsNoTracking().FirstOrDefaultAsync(x => x.TenantId == tenant.Id);

                    if (company != null)
                    {
                        if (tenant.SubscriptionEndTime.HasValue)
                        {
                            var fiveDaysToExpire = tenant.SubscriptionEndTime.GetValueOrDefault().Date == DateTime.Today.AddDays(5);
                            var oneDayToExpire = tenant.SubscriptionEndTime.GetValueOrDefault().Date == DateTime.Today.AddDays(1);
                            if (fiveDaysToExpire || oneDayToExpire)
                            {
                                string href =$"{TenantHelper.GetTenantBaseUrl(tenant.TenancyName)}/{StaticResource.Private.MultiTenant.ManageSubscription.Path.PrefixAngularHashUri()}";

                                string subject = oneDayToExpire
                                    ? $"Renew your subscription within tomorrow"
                                    : $"Your subscription will expire soon";
                                string body =
                                    $"Your subscription will expire on {tenant.SubscriptionEndTime: dd/MM/yyyy}. To continue with {AppSettings.ApplicationName.ToLower().RemoveSpace()} you need to renew or upgrade your subscription. <br/> You can upgrade to a new package or you can renew your existing subscription from <a href='{href}'>Manage Subscription</a><br/>.";


                                await _emailSender.SendSecurityEmailAsync(company.Email, company.Name, subject, body);

                                if (oneDayToExpire && company.IsPhoneConfirmed)
                                {
                                    _smsService.SendOneToOneSingleSmsUsingApi(company.Phone,
                                        SmsHelper.SubscriptionExpireMessage);
                                }
                            }
                        }

                    }

                }
            }
        }


    }
}