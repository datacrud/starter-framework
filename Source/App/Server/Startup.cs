using Microsoft.Owin;
using Owin;
using Project.Model;
using Project.Model.SeedData;
using Project.Server.Hangfire;
using Security.Server.Startup;

[assembly: OwinStartup(typeof(Project.Server.Startup))]

namespace Project.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BusinessDbContext, Model.Migrations.Configuration>());

            app.MapSignalR();

            AuthStartup.Register(app);

            HangfireConfig.RegisterHangfire(app);

            LoggerConfig.Register();

            //BusinessModelSeedDataManager.RunSeed();
            SecurityModelSeedDataManager.RunSeed();
            BusinessModelSeedDataManager.FillTenantSubscription();
            BusinessModelSeedDataManager.CreateFiscalYear(BusinessDbContext.Create());
        }

    }
}
