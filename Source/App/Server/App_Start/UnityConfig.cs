using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Project.Server.Models;
using Unity.WebApi;

namespace Project.Server
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterTypes(AllClasses.FromAssemblies(BuildManager.GetReferencedAssemblies().Cast<Assembly>()),
                WithMappings.FromMatchingInterface, WithName.Default, overwriteExistingMappings: true);

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}