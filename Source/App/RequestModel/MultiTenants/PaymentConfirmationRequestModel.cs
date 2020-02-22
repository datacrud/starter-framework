using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Project.Core.Enums;

namespace Project.RequestModel
{
    public class PaymentConfirmationRequestModel
    {
        [Required]
        public string SubscriptionId { get; set; }

        public SubscriptionPackage Package { get; set; }

        [Required]
        public SubscriptionPaymentStatus PaymentStatus { get; set; }

        [Required]
        public string VerificationCode { get; set; }
    }
}
