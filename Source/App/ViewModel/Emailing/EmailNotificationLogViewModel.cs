using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class EmailNotificationLogViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<EmailLog>
    {
        public EmailNotificationLogViewModel(EmailLog model) : base(model)
        {
           
            Date = model.Date;
            EntityId = model.EntityId;
            EntityInvoiceNo = model.EntityInvoiceNo;
            EntityType = model.EntityType;
            ActionType = model.ActionType;
            EmailSenderUserId = model.EmailSenderUserId;
            EmailReciverUserId = model.EmailReceiverUserId;
            FromEmailAddress = model.FromEmailAddress;
            ToEmailAddress = model.ToEmailAddress;
            Subject = model.Subject;
            Body = model.Body;
            EmailType = model.EmailType;
            IsRead = model.IsRead;
            IsUsedCarryForwardEmail = model.IsUsedCarryForwardEmail;
        }

        public DateTime Date { get; set; }
        public string EntityId { get; set; }
        public string EntityInvoiceNo { get; set; }
        public EntityType EntityType { get; set; }
        public ActionType ActionType { get; set; }

        public string EmailSenderUserId { get; set; }
        public string EmailReciverUserId { get; set; }
        public string FromEmailAddress { get; set; }
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailType EmailType { get; set; }
        public bool IsRead { get; set; }
        public bool IsUsedCarryForwardEmail { get; set; }
    }
}
