using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using Project.Core.Session;
using Project.Model;
using Project.Service.Sms;
using Unity.WebApi;
using Project.Core.Repositories;
using Security.Server.Repository;
using Project.Model.Repositories;
using Project.Service.BackgroundJobs.SubscriptionExpireNotifications;
using Serilog;
using Serilog.Core;
using Security.Models.Models;
using Unity;
using Unity.RegistrationByConvention;
using Unity.Lifetime;

namespace Project.Server
{
    public static class UnityConfig
    {
        public static UnityContainer RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterTypes(AllClasses.FromAssemblies(BuildManager.GetReferencedAssemblies().Cast<Assembly>()),
                WithMappings.FromMatchingInterface, WithName.Default, overwriteExistingMappings: true);

            //container.RegisterTypes(
            //    AllClasses.FromLoadedAssemblies()
            //        .Where(t => t.Name.EndsWith("Mapper")),
            //    WithMappings.None,
            //    WithName.Default,
            //    WithLifetime.ContainerControlled);

            container.RegisterType<IAppSession, AppSession>();
            container.RegisterType<ISubscriptionExpireNotification, SubscriptionExpireNotification>();
            container.RegisterType<ISmsService, OnnoRokomSmsService>();
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<DbContext, BusinessDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<DbContext, SecurityDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType(typeof(IGenericRepository<,>), typeof(GenericRepository<,,>));
            container.RegisterType(typeof(ISecurityRepository<,>), typeof(SecurityRepository<,>));
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            container.RegisterType(typeof(IHaveTenantIdRepositoryBase<>), typeof(HaveTenantIdRepositoryBase<>));
            container.RegisterType(typeof(IHaveTenantIdCompanyIdRepositoryBase<>), typeof(HaveTenantIdCompanyIdRepositoryBase<>));
            container.RegisterType(typeof(IHaveTenantIdCompanyIdBranchIdRepositoryBase<>), typeof(HaveTenantIdCompanyIdBranchIdRepositoryBase<>));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            return container;
        }
    }
}