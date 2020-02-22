using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Core.Extensions;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class AuditLogViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<AuditLog>
    {
        public AuditLogViewModel(AuditLog model) : base(model)
        {
           
            Date = model.Date;
            EntityId = model.EntityId;
            EntityType = model.EntityType;
            EntityTypeName = model.EntityType.GetDescription();
            EntityInvoiceNo = model.EntityInvoiceNo;
            Data = model.Data;
            ActionType = model.ActionType;
            ActionTypeName = model.ActionType.GetDescription();

            ActionedBy = model.Creator?.FullName();
        }

        public DateTime Date { get; set; }
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public string EntityTypeName { get; set; }
        public string EntityInvoiceNo { get; set; }
        public string Data { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionTypeName { get; set; }

        public string ActionedBy { get; set; }
    }
}
