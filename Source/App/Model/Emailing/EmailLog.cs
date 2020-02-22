using System;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class EmailLog : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        public DateTime Date { get; set; }
        public string EntityId { get; set; }
        public string EntityInvoiceNo { get; set; }
        public EntityType EntityType { get; set; }
        public ActionType ActionType { get; set; }

        public string EmailSenderUserId { get; set; }
        public string EmailReceiverUserId { get; set; }
        public string FromEmailAddress { get; set; }
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailType EmailType { get; set; }
        public bool IsRead { get; set; }
        public bool IsUsedCarryForwardEmail { get; set; }
    }
}