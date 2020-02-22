using System;
using System.Threading.Tasks;
using System.Web.Http;
using Hangfire;
using Project.Core.Extensions;
using Project.Core.StaticResource;
using Project.Server.Controllers.Bases;
using Project.Service;
using Project.Service.BackgroundJobs.SubscriptionExpireNotifications;
using Project.ViewModel;
using Security.Server.Service;
using Serilog;

namespace Project.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly ISubscriptionExpireNotification _notificationNotification;
        private readonly IDashboardDataService _service;
        private readonly IUserService _userService;

        public DashboardController(IDashboardDataService dashboardDataService, 
            IUserService userService,
            ISubscriptionExpireNotification notificationNotification)
        {
            _service = dashboardDataService;
            _userService = userService;
            _notificationNotification = notificationNotification;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            await Task.Delay(0);

            var viewModel = _service.GetData();
            var userAlerts = _userService.CheckUserAlert(User.Identity.GetTenantId());
            viewModel.DashboardAlerts.AddRange(userAlerts.ConvertAll(x=> new DashboardAlertViewModel(x)));

            if (!User.IsInRole(StaticRoles.Admin.Name) && !User.IsInRole(StaticRoles.Manager.Name))
            {
                viewModel.SaleToday = 0;
                viewModel.DepositToday = 0;
                viewModel.ExpenseToday = 0;
            }
            

            RegisterRecurringJobs();

            return Ok(viewModel);
        }


        private void RegisterRecurringJobs()
        {
            try
            {
                RecurringJob.AddOrUpdate("SubscriptionExpireNotifier",
                    () => _notificationNotification.ExecuteNotify(), Cron.Daily);
            }
            catch (Exception e)
            {
                Log.Error(e.ToStringOrDefault());
            }

        }  
        
              

    }

}
