using System;
using Unity;

namespace Project.Core.IoC
{
    public class IocManager
    {
        private static readonly IUnityContainer Container;

        static IocManager()
        {
            IUnityContainer container = new UnityContainer();

            //var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            //section?.Configure(container);

            Container = container;
        }

        public static T Resolve<T>()
        {
            T resolved;

            //if (Container.IsRegistered<T>()) 
            try
            {
                resolved = Container.Resolve<T>();
            }
            catch (Exception)
            {
                resolved = default(T);
            }
            return resolved;
        }

        public static void RegisterType(Type from, Type to)
        {
            Container.RegisterType(from, to);
        }

    }
}
