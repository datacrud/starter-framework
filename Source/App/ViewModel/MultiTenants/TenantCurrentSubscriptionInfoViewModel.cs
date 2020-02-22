using System;
using Project.Core.Enums;
using Project.Core.Extensions;

namespace Project.ViewModel
{
    public class TenantCurrentSubscriptionInfoViewModel
    {
        public string SubscriptionId { get; set; }

        public string EditionName { get; set; }

        public SubscriptionPackage Package { get; set; }

        public int NoOfShowroom { get; set; }

        public DateTime? SubscriptionEndTime { get; set; }


        public string Message => GetMessage();

        private string GetMessage()
        {
            var showroomString = NoOfShowroom == 0 ? "Unlimited Showrooms" :
                NoOfShowroom == 1 ? $"{NoOfShowroom} Showroom" : $"{NoOfShowroom} Showrooms";

            string message =
                $"Currently your are subscribed at <strong>Subscription ID({SubscriptionId})</strong> of <strong>{EditionName} Edition</strong> with <strong>{Package.GetDescription()} Package</strong> bearing <strong>{showroomString}</strong>. ";

            message += SubscriptionEndTime.HasValue && Package != SubscriptionPackage.LifeTime
                ? $"Your subscription will expire on <strong>{SubscriptionEndTime: dd MMMM yyyy}</strong>"
                : $"You got a life time subscription which will never expire.";


            return message;
        }
    }
}