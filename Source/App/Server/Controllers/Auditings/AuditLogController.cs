using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Model;
using Project.RequestModel;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.ViewModel;

namespace Project.Server.Controllers
{
    public class AuditLogController : HaveTenantIdCompanyIdBranchIdControllerBase<AuditLog, AuditLogViewModel, AuditLogRequestModel>
    {
        private readonly IAuditLogService _service;

        public AuditLogController(IAuditLogService service) : base(service)
        {
            _service = service;
        }

    }
}
