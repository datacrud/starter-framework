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
    public class LoginAttemptController : HaveTenantIdCompanyIdBranchIdControllerBase<LoginAttempt, LoginAttemptViewModel, LoginAttemptRequestModel>
    {
        private readonly ILoginAttemptService _service;


        public LoginAttemptController(ILoginAttemptService service) : base(service)
        {
            _service = service;
        }
    }
}
