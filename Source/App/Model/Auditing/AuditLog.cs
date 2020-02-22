using System;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class AuditLog : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public string EntityInvoiceNo { get; set; }
        public string Data { get; set; }
        public ActionType ActionType { get; set; }

    }
}