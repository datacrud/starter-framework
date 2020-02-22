using Hangfire;
using Owin;
using Project.Core.Repositories;
using Project.Server.Filters;

namespace Project.Server.Hangfire
{
    public class HangfireConfig
    {
        public static void RegisterHangfire(IAppBuilder app)
        {
            var container = UnityConfig.RegisterComponents();

            GlobalConfiguration.Configuration.UseActivator(new HangfireUnityJobActivator(container));
            GlobalConfiguration.Configuration.UseSqlServerStorage(ConnectionStringProvider.DefaultConnection);


            var options = new DashboardOptions()
            {
                Authorization = new[]
                {
                    new HangfireDashboardAuthorizationFilter
                    {
                        //Users = "systemadmin", Roles = "SystemAdmin"
                    }
                }
            };

            app.UseHangfireDashboard("/hangfire", options);
            app.UseHangfireServer();


        }
    }
}